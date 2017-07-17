using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SistemaAcademico.Classes;
using SistemaAcademico.DataModel;

namespace SistemaAcademico.Controllers.Api
{
    public class PreseleccionController : ApiController
    {
        private int currentPeriod { get; set; }
        public PreseleccionController()
        {
            using (var context = new AcademicSystemContext())
            {
                currentPeriod = context.Periodos.Where(p => p.Status == SchemaTypes.PeriodStatus.En_Curso).First().PeriodoID;
            }
        }
        public class ofertaDTO
        {
            public int id { get; set; }
            public string Name { get; set; }
            public string Codigo { get; set; }
        }
        public object GetOferta(int id, string query)
        {
            using (var context = new AcademicSystemContext())
            {


                var querySet = context.Database.SqlQuery<ofertaDTO>(
                "SELECT [Asignatura_AsignatureID] as 'id',[Name], [Codigo]"
               + "FROM [dbo].[PeriodAsignatures] "
               + "JOIN [dbo].[Asignaturas] "
               + "ON [Asignatura_AsignatureID] = [AsignatureID] "
               + "WHERE [Periodo_PeriodoID] = " + id.ToString()
               + "AND [Name] LIKE '%" + query + "%'"
               + " GROUP BY [Asignatura_AsignatureID],[Name] ,[Codigo]"
                                                );
                var result = querySet
                                    .Take(20)
                                    .ToList();
                return result;


            }
        }

        public IEnumerable<ofertaDTO> getAsignaturasOfertadas()
        {
            using (var context = new AcademicSystemContext())
            {

                return context.Database.SqlQuery<ofertaDTO>(
                            "SELECT [Asignatura_AsignatureID] as 'id',[Name], [Codigo]"
                           + "FROM [dbo].[PeriodAsignatures] "
                           + "JOIN [dbo].[Asignaturas] "
                           + "ON [Asignatura_AsignatureID] = [AsignatureID] "
                           + "WHERE [Periodo_PeriodoID] = " + currentPeriod.ToString()
                           + " GROUP BY [Asignatura_AsignatureID],[Name] ,[Codigo]"
                    ).ToList();
            }


        }


        [HttpGet]
        public object getOfertaDetallada()
        {
            using (var context = new AcademicSystemContext())
            {
                List<object> values = new List<object>();
                var asignaturas = getAsignaturasOfertadas();
                foreach (var asignatura in asignaturas)
                {
                    var data = new
                    {
                        asignatura = asignatura,
                        secciones = context.PeriodAsignature
                        .Where(p => p.Periodo.PeriodoID == currentPeriod)
                                        .Select(d => new
                                        {
                                            seccion = d.PeriodAsignatureID,
                                            horarios = context.Horarios.Where(h => h.Asignatura.PeriodAsignatureID == d.PeriodAsignatureID).ToList()
                                        }).ToList()
                    };

                    values.Add(data);
                }
                return values;

            }
        }

        [HttpGet]
        public object getSeccionesByMateria(int MateriaID)
        {
            using (var context = new AcademicSystemContext())
            {


                var data = context.PeriodAsignature.Where(p => p.Asignatura.AsignatureID == MateriaID && p.Periodo.PeriodoID == currentPeriod)
                       .Select(d => new
                       {
                           seccion = d.PeriodAsignatureID,
                           horarios = context.Horarios.Where(h => h.Asignatura.PeriodAsignatureID == d.PeriodAsignatureID).Select(p =>new
                           {
                               inicio = p.HourInit,
                               fin = p.HourUntil,
                               room = p.Aula.Building.ToUpper() + "-"+ p.Aula.RoomNumber.ToString()
                           }).ToList(),
                           maestro = d.Profesor.Name + " " + d.Profesor.Name + " " + d.Profesor.LastName
                       }).ToList();
                
                return data;

            }
        }

        public object GetStudentCurrent(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                var currPeriod = context.Periodos.Where(p => p.Status == SchemaTypes.PeriodStatus.En_Curso).First();

                if (currPeriod.fechafinPreseleccion > DateTime.Now)
                {
                    int currP = currPeriod.PeriodoID;


                    var preseleccion = context
                                          .Preselecciones
                                             .Where(p => p.Student.UserId == id && p.Asignatura.Periodo.PeriodoID == currP)
                                             .Select(d => new
                                             {
                                                 materia = d.Asignatura.Asignatura.Name,
                                                 codigo = d.Asignatura.Asignatura.Codigo,
                                                 materiaId = d.Asignatura.Asignatura.AsignatureID,
                                                 tanda = d.TandaDeseada,
                                                 lineaID = d.PreseleccionID
                                             }).ToList();
                    return preseleccion;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden);
                }

            }
        }

        public class PreseleccionDTO
        {
            public int userID { get; set; }
            public int AsignatureID { get; set; }
            public FixedValues.Tanda Tanda { get; set; }
        }
        public object PostLine(PreseleccionDTO @new)
        {
            using (var context = new AcademicSystemContext())
            {
                int currP = context.Periodos.Where(p => p.Status == SchemaTypes.PeriodStatus.En_Curso).First().PeriodoID;
                context.Preselecciones.Add(new Preseleccion
                {
                    Asignatura = context.PeriodAsignature.Where(p => p.Periodo.PeriodoID == currP && p.Asignatura.AsignatureID == @new.AsignatureID).First(),
                    Student = context.Usuarios.Where(u => u.UserId == @new.userID).First(),
                    TandaDeseada = @new.Tanda

                });
                context.SaveChanges();
            }
            return HttpStatusCode.OK;
        }

        // GET: api/Preseleccion/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Preseleccion
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Preseleccion/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Preseleccion/5
        public void Delete(int id)
        {
        }
    }
}
