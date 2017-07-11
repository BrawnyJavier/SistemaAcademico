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
                    + "AND [Name] LIKE '%"+query+"%'"
                    + " GROUP BY [Asignatura_AsignatureID],[Name] ,[Codigo]"
                                                     );
                var result = querySet
                                    .Take(20)
                                    .ToList();
                return result;
            }
        }
        public object GetStudentCurrent(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                int currP = context.Periodos.Where(p => p.Status == SchemaTypes.PeriodStatus.En_Curso).First().PeriodoID;

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
