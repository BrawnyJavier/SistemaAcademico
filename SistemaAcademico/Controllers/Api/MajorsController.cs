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
    public class MajorsController : ApiController
    {
        // GET: api/Majors
        public IEnumerable<object> Get()
        {
            using (var c = new SistemaAcademico.DataModel.AcademicSystemContext())
            {
                return c.Majors.Select(d => new
                {
                    area = d.Area.Name,
                    MajorTitle = d.MajorTitle,
                    fechainicio = d.FechaIntroduccion,
                    id = d.MajorID,
                    description = d.Description,
                    CreditsCount = d.CreditsCount

                }).ToList();

            }
        }

        // GET: api/Majors/5
        public string Get(int id)
        {
            return "value";
        }

        public class MajorDTO
        {
            public int? MajorID { get; set; }
            public string MajorTitle { get; set; }
            public string Area { get; set; }
            public string description { get; set; }
            public int CreditsCount { get; set; }
        }
        public void Post(MajorDTO @new)
        {
            using (var c = new SistemaAcademico.DataModel.AcademicSystemContext())
            {
                var newMajor = new Classes.Major
                {
                    MajorTitle = @new.MajorTitle,
                    FechaIntroduccion = DateTime.Now,
                    Area = c.Areas.Where(a => a.Name == @new.Area).FirstOrDefault(),
                    CreditsCount = @new.CreditsCount
                };
                if (@new.description == null) newMajor.Description = "Pendiente".ToUpper();
                else newMajor.Description = @new.description;

                c.Majors.Add(newMajor);
                c.SaveChanges();
            }
        }
        public HttpResponseMessage Put(MajorDTO toUpdate)
        {
            using (var context = new SistemaAcademico.DataModel.AcademicSystemContext())
            {
                var majorInDb = context.Majors.Where(m => m.MajorID == toUpdate.MajorID).FirstOrDefault();
                majorInDb.Description = toUpdate.description;
                majorInDb.MajorTitle = toUpdate.MajorTitle;
                majorInDb.Area = context.Areas.Where(a => a.Name == toUpdate.Area).FirstOrDefault();
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        public class pensumLineDTO
        {
            public int AsignaturaID { get; set; }
            public int MajorID { get; set; }
        }
        [HttpPost]
        public HttpStatusCode addPensumLine(pensumLineDTO PensumLine)
        {
            using (var context = new AcademicSystemContext())
            {
                context.Pensums.Add(new Pensum
                {
                    Asignatura = context.Asignaturas.Where(a => a.AsignatureID == PensumLine.AsignaturaID).FirstOrDefault(),
                    Major = context.Majors.Where(m => m.MajorID == PensumLine.MajorID).FirstOrDefault(),
                    TrimestreOrder = 1
                });
                context.SaveChanges();
                return HttpStatusCode.OK;
            }
        }
        [HttpDelete]
        public HttpStatusCode deletePensumLine(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                context.Pensums.Remove(context.Pensums.Where(p => p.PensumID == id).FirstOrDefault());
                context.SaveChanges();
                return HttpStatusCode.OK;
            }
        }



        [HttpGet]
        public object getPensum(int MajorID)
        {
            using (var context = new AcademicSystemContext())
            {
                var queryResult =
                            context
                            .Pensums.Where(p => p.Major.MajorID == MajorID)
                            .Select(d => new
                            {
                                clave = d.Asignatura.Codigo,
                                id = d.PensumID,
                                asignatura = d.Asignatura.Name,
                                creditos = d.Asignatura.Credits,
                                prereqAsigs = context.Requisitos.Where(r => r.Asignatura.AsignatureID == d.Asignatura.AsignatureID && r.TipoRequisito == SchemaTypes.RequisitoTypes.Asignatura).ToList(),
                                prereqCreds = context.Requisitos.Where(r => r.Asignatura.AsignatureID == d.Asignatura.AsignatureID && r.TipoRequisito == SchemaTypes.RequisitoTypes.Creditos).FirstOrDefault()
                            }).ToList();

                return queryResult;

            }
        }
        // DELETE: api/Majors/5
        public void Delete(int id)
        {
        }
    }
}
