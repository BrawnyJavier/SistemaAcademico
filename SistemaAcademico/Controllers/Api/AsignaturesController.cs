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
    public class AsignaturesController : ApiController
    {
        [HttpGet]
        public IEnumerable<Object> getAllAsignaturas()
        {
            using (var context = new SistemaAcademico.DataModel.AcademicSystemContext())
            {
                var d = context.Asignaturas.Select(a => new
                {
                    Nombre = a.Name,
                    Creditos = a.Credits,
                    ID = a.AsignatureID,
                    Tipo = a.TipoAsignatura
                }).Select(dd => new
                {
                    nombre = dd.Nombre,
                    tipo = (dd.Tipo == 0) ? "Teoria" : "Laboratorio",
                    id = dd.ID,
                    creditos = dd.Creditos
                }).ToList();
                return d;
            }
        }
        [HttpGet]
        public object getAsignatura(int id)
        {
            using (var c = new AcademicSystemContext())
            {
                return c.Asignaturas
                    .Where(a => a.AsignatureID == id)
                    .Select(s => new
                    {
                        s.Codigo,
                        s.Credits,
                        s.Name,
                        s.TipoAsignatura,
                        teachers = c.AsignatureTeachers
                                   .Where(at => at.Asignatura.AsignatureID == s.AsignatureID)
                                   .Select(data => new
                                   {
                                       asignID = data.id,
                                       data.AsignDate,
                                       teacherID = data.Teacher.UserId,
                                       Teacher = data.Teacher.Name + " " + data.Teacher.Name2 + " " + data.Teacher.LastName
                                   }).ToList()
                    })
                    .FirstOrDefault();
            }
        }


        [HttpDelete]
        public object DeleteTeacher(int asignid)
        {
            using (var context = new AcademicSystemContext())
            {
                context.AsignatureTeachers.Remove(context.AsignatureTeachers.Where(at => at.id == asignid).FirstOrDefault());
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        public class AsignaturaDTO
        {
            public string NombreAsignatura { get; set; }
            public int Creditos { get; set; }
            public string Codigo { get; set; }
            public SchemaTypes.AsignatureTypes TipoAsignatura { get; set; }

        }
        [HttpPost]
        public HttpResponseMessage createAsignatura(AsignaturaDTO @new)
        {
            try
            {
                using (var c = new AcademicSystemContext())
                {
                    c.Asignaturas.Add(new Asignatura
                    {
                        Credits = @new.Creditos,
                        Name = @new.NombreAsignatura,
                        TipoAsignatura = @new.TipoAsignatura,
                        Codigo = @new.Codigo
                    });
                    c.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
            }
            catch (Exception e)
            {
                if (e.InnerException != null) return Request.CreateResponse(HttpStatusCode.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }
        [HttpGet]
        public object fetchAsignatures(string query)
        {
            using (var context = new AcademicSystemContext())
            {
                return context.Asignaturas.Where(a =>
                       (a.Codigo + a.Name).Contains(query)).Select(d => new
                       {
                           codigo = d.Codigo,
                           nombre = d.Name,
                           creditos = d.Credits,
                           id = d.AsignatureID
                       }).ToList();

            }
        }
        [HttpGet]
        public object fetchAsignaturas(string query)
        {
            using (var context = new AcademicSystemContext())
            {
                var d = context.Asignaturas
                    .Where(ad => ad.Name.Contains(query) || ad.Codigo.Contains(query))

                    .Select(a => new
                    {
                        Nombre = a.Name,
                        Creditos = a.Credits,
                        ID = a.AsignatureID,
                        Tipo = a.TipoAsignatura,
                        codigo = a.Codigo
                    }).Select(dd => new
                    {
                        nombre = dd.Nombre,
                        tipo = (dd.Tipo == 0) ? "Laboratorio" : "Teoria",
                        id = dd.ID,
                        creditos = dd.Creditos,
                        codigo = dd.codigo
                    }).ToList();
                return d;
            }
        }
    }
}
