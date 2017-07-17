using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SistemaAcademico;
using SistemaAcademico.DataModel;
using SistemaAcademico.Classes;

namespace SistemaAcademico.Controllers.Api
{
    public class StudentsMajorsController : ApiController
    {
        // GET: api/StudentsMajors
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/StudentsMajors/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        public object getStudentsMajors(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                return context.StudentMajors
                               .Where(sm => sm.Student.UserId == id)
                               .Select(d => new
                               {
                                   name = d.Major.MajorTitle,
                                   id = d.Major.MajorID,
                               }).ToList();
            }
        }

        [HttpGet]
        public object getHistorialAcademico(int userMajor)
        {
            using (var context = new AcademicSystemContext())
            {
                var result = context.StudentsHistories
                                    .Where(h => h.StudentMajor.id == userMajor)
                                    .Select(d => new
                                    {
                                        PeriodoIDt = d.Asignatura.Periodo.PeriodoID,
                                        PeriodInit = d.Asignatura.Periodo.fechaInicio,
                                        PeriodFin = d.Asignatura.Periodo.fechaFin,
                                        calificaciones = context.StudentsHistories
                                                                .Where(hm => 
                                                                            hm.StudentMajor.id == userMajor
                                                                            && hm.Asignatura.Periodo.PeriodoID == d.Asignatura.Periodo.PeriodoID
                                                                      )
                                                                .Select(c => new
                                                                {
                                                                    Codigo = c.Asignatura.Asignatura.Codigo,
                                                                    seccion = c.Asignatura.seccion,
                                                                    NombreAsig = c.Asignatura.Asignatura.Name,
                                                                    Calif = c.Calificacion,
                                                                    creditos = c.Asignatura.Asignatura.Credits
                                                                }).ToList()
                                    }).ToList();
                return result;
            }
        }






        public class StudentMajorDTO
        {
            public int StudentID { get; set; }
            public int MajorID { get; set; }
        }
        public object Post(StudentMajorDTO @new)
        {
            using (var context = new AcademicSystemContext())
            {
                StudentMajor newSM = new StudentMajor();
                newSM.Student = context.Usuarios
                    .Where(u => u.UserId == @new.StudentID)
                    .FirstOrDefault();
                newSM.InitDate = DateTime.Now;
                newSM.Status = SchemaTypes.StudentMajorStatus.Cursando;
                newSM.Major = context.Majors.Where(m => m.MajorID == @new.MajorID).FirstOrDefault();
                newSM.GPA = 0;
                context.StudentMajors.Add(newSM);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);

            }
        }

        // PUT: api/StudentsMajors/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/StudentsMajors/5
        public void Delete(int id)
        {
        }
    }
}
