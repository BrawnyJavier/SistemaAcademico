using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SistemaAcademico.Classes;
using SistemaAcademico.DataModel;
using SistemaAcademico.SchemaTypes;
using System.Threading;

namespace SistemaAcademico.Controllers.Api
{
    public class SeleccionController : ApiController
    {
        private int currentPeriod { get; set; }
        public SeleccionController()
        {
            using (var context = new AcademicSystemContext())
            {
                currentPeriod = context.Periodos.Where(p => p.Status == SchemaTypes.PeriodStatus.En_Curso).First().PeriodoID;
            }
        }
        // GET: api/Seleccion
        public object GetStudentCurrent(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                var currPeriod = context.Periodos.Where(p => p.Status == SchemaTypes.PeriodStatus.En_Curso).First();

                if (currPeriod.fechafinPreseleccion > DateTime.Now)
                {
                    int currP = currPeriod.PeriodoID;

                    var seleccion = context.StudentsHistories
                        .Where(h => h.StudentMajor.Student.UserId == id && h.Asignatura.Periodo.PeriodoID == currP && h.Status != SchemaTypes.HistorialStatus.Cancelada)
                        .Select(d => new
                        {
                            materia = d.Asignatura.Asignatura.Name,
                            codigo = d.Asignatura.Asignatura.Codigo,
                            materiaID = d.Asignatura.Asignatura.AsignatureID,
                            horario = d.Asignatura.Horarios.ToList()

                        }).ToList();

                    return seleccion;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden);
                }

            }
        }

        // GET: api/Seleccion/5
        public string Get(int id)
        {
            return "value";
        }


        // POST: api/Seleccion
        [HttpPost]
        [SistemaAcademico.Services.SisAcademicoFilter]
        public object PostHistorial(int seccionID)
        {
            int currser = int.Parse(Thread.CurrentPrincipal.Identity.Name);

            using (var context = new AcademicSystemContext())
            {
                StudentHistory newLine = new StudentHistory
                {

                    Asignatura = context.PeriodAsignature
                         .Where(a => a.PeriodAsignatureID == seccionID).FirstOrDefault(),

                    StudentMajor = context.StudentMajors
                            .Where(e => e.Student.UserId == currser && e.Status == StudentMajorStatus.Cursando).First(),

                    Status = HistorialStatus.En_Curso

                };

                context.StudentsHistories.Add(newLine);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, newLine);
            }
        }

        [HttpGet]
        [SistemaAcademico.Services.SisAcademicoFilter]
        public object getStudentSelection()
        {
            int currser = int.Parse(Thread.CurrentPrincipal.Identity.Name);
            using (var context = new AcademicSystemContext())
            {
                var result = context.StudentsHistories
                    .Where(h => h.StudentMajor.Student.UserId == currser && h.Asignatura.Periodo.PeriodoID == currentPeriod)
                    .Select(data => new
                    {
                        asignatura = data.Asignatura.Asignatura.AsignatureID,
                        codigo = data.Asignatura.Asignatura.Codigo,
                        seccion = data.Asignatura.seccion,
                        asigname = data.Asignatura.Asignatura.Name,
                        profesor = data.Asignatura.Profesor.Name + " "+ data.Asignatura.Profesor.LastName,
                     
                    }).ToList();
                return result;
            }
        }
        // PUT: api/Seleccion/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Seleccion/5
        public void Delete(int id)
        {
        }
    }
}
