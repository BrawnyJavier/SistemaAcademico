using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SistemaAcademico.DataModel;
using SistemaAcademico.Classes;
namespace SistemaAcademico.Controllers.Api
{
    public class TeachersAsignaturesController : ApiController
    {
        public class TeacherAsignatureDTO
        {
            public int TeacherID { get; set; }
            public int AsignatureID { get; set; }
        }
        [HttpPost]
        public HttpResponseMessage Post(TeacherAsignatureDTO @newTAsignature)
        {
            using (var context = new AcademicSystemContext())
            {
                var newTA = new AsignatureTeacher
                {
                    Asignatura = context.Asignaturas
                                 .Where(a => a.AsignatureID == newTAsignature.AsignatureID)
                                 .FirstOrDefault(),
                    AsignDate = DateTime.Now,
                    Teacher = context.Usuarios
                                .Where(t => t.UserId == newTAsignature.TeacherID && t.UserType == SchemaTypes.UserTypes.Teacher)
                                .FirstOrDefault()
                };
                context.AsignatureTeachers.Add(newTA);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created);
            }
        }

        [HttpGet]
        public object getAsignatureTeachers(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                return context.AsignatureTeachers.Where(a => a.Asignatura.AsignatureID == id).Select(d => new
                {
                    d.Teacher.UserId,
                    d.Teacher.Name,
                    d.Teacher.Name2,
                    d.Teacher.LastName
                }).ToList();
            }
        }
    }
}
