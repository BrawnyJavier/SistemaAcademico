using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SistemaAcademico.DataModel;
using SistemaAcademico.Classes;
using System.Threading;

namespace SistemaAcademico.Controllers.Api
{
    public class AutoSeleccionController : ApiController
    {
        public class Preselecciones
        {
            public int AsignatureID { get; set; }
            public int Qntity { get; set; }
            public FixedValues.Tanda TandaDeseada { get; set; }
            public int HorasSemanales { get; set; }
        }
        public class posibleTeacher
        {
            public int UserId { get; set; }
        }
        [HttpPost]
        public object createSecciones()
        {
            using (var context = new AcademicSystemContext())
            {
                // Traera las asignaturas preseleccionadas, y sus respectivas tandas, mas la cantidad de estudiantes q la preseleccionaron
                string Query = (
                      "SELECT A.AsignatureID,A.[HorasSemanales], count(*) as Qntity, P.TandaDeseada "
                    + "FROM [dbo].[Preseleccions] P "
                    + "INNER JOIN [dbo].[PeriodAsignatures] PA "
                    + "ON p.Asignatura_PeriodAsignatureID = PA.PeriodAsignatureID "
                    + "INNER JOIN [dbo].[Asignaturas] A "
                    + "ON A.AsignatureID = PA.Asignatura_AsignatureID "
                    + "WHERE PA.Periodo_PeriodoID = (SELECT PeriodoID FROM Periodoes WHERE STATUS = 1) "
                    + "GROUP BY A.AsignatureID, p.TandaDeseada,A.[HorasSemanales]; "
                    );
                IEnumerable<Preselecciones> PreseleccionesDelTrimestreActual = context.Database.SqlQuery<Preselecciones>(Query).ToList<Preselecciones>();

                int[] DAYS = { 1, 2, 3, 4, 5, 6 }; //Lunes, Martes ... Sabado

                List<Room> Rooms = context.Rooms.ToList();
                List<PeriodAsignature> SeccionesCreadas = new List<PeriodAsignature>();

                foreach (Preselecciones preseleccion in PreseleccionesDelTrimestreActual)
                {
                    // Minimun 10 Students per sections.
                    if (preseleccion.Qntity > 0)
                    {
                        PeriodAsignature nuevaSeccion = new PeriodAsignature
                        {
                            Asignatura = context.Asignaturas.Where(a => a.AsignatureID == preseleccion.AsignatureID).FirstOrDefault(),
                            Periodo = context.Periodos.Where(p => p.Status == SchemaTypes.PeriodStatus.En_Curso).FirstOrDefault(),
                            Tanda = preseleccion.TandaDeseada,
                            Status = SchemaTypes.PeriodAsignatureStatus.EnConfeccion,
                            seccion = context
                                .PeriodAsignature
                                .Where(a => a.Asignatura.AsignatureID == preseleccion.AsignatureID
                                                                                                    && a.Periodo.PeriodoID == context.Periodos
                                                                                                                .Where(p => p.Status == SchemaTypes.PeriodStatus.En_Curso)
                                                                                                                .Select(p => p.PeriodoID)
                                                                                                                .FirstOrDefault())
                                .Count() + 1,
                        };
                        context.PeriodAsignature.Add(nuevaSeccion);

                        context.SaveChanges();
                        SeccionesCreadas.Add(nuevaSeccion);

                        List<Horario> horariosNuevaSeccion = new List<Horario>();

                        int horas = preseleccion.HorasSemanales;
                        int maxHoursPerDay = 2;
                        int remainingHours = preseleccion.HorasSemanales;

                        // Buscamos los limites de la tanda
                        DateTime horaInicio = (preseleccion.TandaDeseada == FixedValues.Tanda.Matutina) ? new DateTime(2017, 1, 1, 7, 0, 0)
                                              : (preseleccion.TandaDeseada == FixedValues.Tanda.Vespertina) ? new DateTime(2017, 1, 1, 13, 0, 0)
                                              : new DateTime(2017, 1, 1, 18, 0, 0);
                        DateTime Limite = (preseleccion.TandaDeseada == FixedValues.Tanda.Matutina) ? new DateTime(2017, 1, 1, 13, 0, 0)
                                              : (preseleccion.TandaDeseada == FixedValues.Tanda.Vespertina) ? new DateTime(2017, 1, 1, 18, 0, 0)
                                              : new DateTime(2017, 1, 1, 22, 0, 0);

                        // Buscamos horarios que hagan match con las aulas, es decir, en que horas de que dia esta la aula desocupada.
                        foreach (int day in DAYS)
                        {
                            if (remainingHours == 0) break;


                            foreach (Room room in Rooms)
                            {
                                DateTime LimiteInicio = horaInicio;
                                if (remainingHours == 0) break;
                                while (LimiteInicio < Limite && remainingHours != 0)
                                {
                                    string hinit = LimiteInicio.ToString();
                                    string hfin = LimiteInicio.AddHours(maxHoursPerDay).ToString();

                                    // Verificamos la disponibilidad del aula
                                    string QueryValidator = (
                                          " DECLARE @hourInit Varchar(max) = '" + hinit + "'; "
                                        + " DECLARE @hourFin Varchar(max) = '" + hfin + "'; "
                                        + " DECLARE @roomID int = " + room.RoomID + ";  "
                                        + " DECLARE @day int = " + day + "; "
                                        + " SELECT H.HorarioID "
                                        + " FROM [dbo].[Horarios] H "
                                        + " WHERE (H.HourInit <= @hourInit) and (H.HourUntil >= @hourFin) "
                                        + " AND H.Aula_RoomID = @roomID AND H.Day = @day;"
                                        );
                                    var results = context.Database.SqlQuery<int>(QueryValidator).ToList();
                                    if (results.Count == 0)
                                    {
                                        remainingHours = remainingHours - maxHoursPerDay;
                                        var horario = new Horario
                                        {
                                            Asignatura = nuevaSeccion,
                                            Day = day,
                                            HourInit = horaInicio,
                                            Aula = context.Rooms.Where(r => r.RoomID == room.RoomID).FirstOrDefault(),
                                            HourUntil = LimiteInicio.AddHours(maxHoursPerDay),

                                        };
                                        context.Horarios.Add(horario);
                                        context.SaveChanges();
                                        break;
                                    }
                                    else
                                    {
                                        // aumentamos la hora en la tanda.
                                        DateTime newDate = LimiteInicio.AddHours(maxHoursPerDay);
                                        LimiteInicio = newDate;
                                    }

                                }

                            }

                        }
                        // Profesores asignados a esta materia
                        var posibleTeachers = context.AsignatureTeachers
                                                       .Where(ap =>
                                                                     ap.Asignatura.AsignatureID == preseleccion.AsignatureID
                                                                     &&
                                                                    (ap.Teacher.tandaProfesor == preseleccion.TandaDeseada || ap.Teacher.tandaProfesor == FixedValues.Tanda.TiempoCompleto)
                                                              )
                                                              .Select(d => d.Teacher.UserId).ToList();
                        bool hasTeacher = false;
                        // Buscamos al profesor que pueda impartir esta materia con los horarios seleccionados anteriormente
                        foreach (int teacher in posibleTeachers)
                        {
                            if (hasTeacher) break;
                            bool Qualifies = false;
                            var horarios_NuevaSeccion = context.Horarios.Where(h => h.Asignatura.PeriodAsignatureID == nuevaSeccion.PeriodAsignatureID).ToList();
                            foreach (var horario in horarios_NuevaSeccion)
                            {
                                string teacherValidator = (
                                                                  " DECLARE @hourInitial Varchar(max) = '" + horario.HourInit.ToString() + "'; "
                                                                + " DECLARE @hourFinalization Varchar(max) = '" + horario.HourUntil.ToString() + "'; "
                                                                + " DECLARE @Dia Int = " + horario.Day + ";  "
                                                                + " DECLARE @UserID Int = " + teacher + "; "
                                                                + " SELECT U.UserId "
                                                                + " FROM  [dbo].[Users] U "
                                                                + " INNER JOIN [dbo].[PeriodAsignatures] PA "
                                                                + " ON PA.Profesor_UserId = U.UserId "
                                                                + " INNER JOIN Horarios H "
                                                                + " ON H.Asignatura_PeriodAsignatureID = Pa.PeriodAsignatureID "
                                                                + " WHERE PA.Periodo_PeriodoID = (SELECT PeriodoID FROM Periodoes WHERE STATUS = 1) "
                                                                + " AND U.UserId = @UserID "
                                                                + " AND (H.HourInit <= @hourInitial) AND (H.HourUntil >= @hourFinalization) "
                                                                + " AND H.Day = @Dia "
                                                                );
                                List<posibleTeacher> resultset = context.Database.SqlQuery<posibleTeacher>(teacherValidator).ToList();
                                if (resultset.Count == 0) Qualifies = true;
                                else Qualifies = false;
                            }
                            if (Qualifies)
                            {
                                nuevaSeccion.Profesor = context.Usuarios.Where(u => u.UserId == teacher).FirstOrDefault();
                                context.SaveChanges();
                                hasTeacher = true;
                            }

                        }
                        if (hasTeacher) // Si encontramos al profesor que pueda cumplir con el horario, creamos la seccion.
                        {
                            var periodoActual = context.Periodos
                                                            .Where(pd => pd.Status == SchemaTypes.PeriodStatus.En_Curso)
                                                                .Select(pid => pid.PeriodoID).FirstOrDefault();


                            var estudiantesNuevaSeccion = context
                                                            .Preselecciones
                                                            .Where(p =>
                                                                       p.Asignatura.Periodo.PeriodoID == periodoActual
                                                                        && p.Asignatura.Asignatura.AsignatureID == preseleccion.AsignatureID
                                                                        && p.TandaDeseada == preseleccion.TandaDeseada
                                                                    )
                                                            .Select(d => d.Student.UserId)
                                                            .ToList();

                            // Añadimos a los estudiantes que preseleccionaron la asignatura en esta tanda, a la seccion recien creada.
                            foreach (int IdEstudiante in estudiantesNuevaSeccion)
                            {
                                var studentM = context.StudentMajors.Where(m => m.Student.UserId == IdEstudiante && m.Status == SchemaTypes.StudentMajorStatus.Cursando).FirstOrDefault();
                                var Inscription = new StudentHistory
                                {
                                    Status = SchemaTypes.HistorialStatus.En_Curso,
                                    Asignatura = nuevaSeccion,
                                    StudentMajor = studentM,
                                    Calificacion = 0,

                                };
                                context.StudentsHistories.Add(Inscription);
                                context.SaveChanges();
                            }
                            nuevaSeccion.Status = SchemaTypes.PeriodAsignatureStatus.Creada;
                            context.SaveChanges();

                        }
                    }
                }

                //var DummySections = context.PeriodAsignature.Where(p => p.Status == SchemaTypes.PeriodAsignatureStatus.EnConfeccion).ToList();
                //foreach (var seccion in DummySections)
                //{
                //    context.PeriodAsignature.Remove(seccion);

                //}
                //context.SaveChanges();


                return SeccionesCreadas.Select(d => new
                {
                    d.PeriodAsignatureID,
                    teacher = (context.PeriodAsignature.Where(pa => pa.PeriodAsignatureID == d.PeriodAsignatureID).FirstOrDefault().Profesor == null) ? "Pendiente" : context.PeriodAsignature.Where(pa => pa.PeriodAsignatureID == d.PeriodAsignatureID).Select(t => t.Profesor.Name + " " + t.Profesor.LastName).FirstOrDefault(),
                    seccion = d.seccion,
                    asignatura = context.Asignaturas.Where(a => a.AsignatureID == context.PeriodAsignature.Where(pa => pa.PeriodAsignatureID == d.PeriodAsignatureID).FirstOrDefault().Asignatura.AsignatureID).Select(m => m.Codigo + "-" + m.Name).FirstOrDefault(),
                    tanda = d.Tanda.ToString()

                }).ToList();
            }

        }
    }
}
