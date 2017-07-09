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
    public class PeriodosController : ApiController
    {
        // GET: api/Periodos
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Periodos/5
        public string Get(int id)
        {
            return "value";
        }

        public class PeriodoDTO
        {
            public DateTime fechaInicio { get; set; }
            public DateTime fechaFin { get; set; }
            public DateTime fechaInicioPreselecion { get; set; }
            public DateTime fechafinPreseleccion { get; set; }
            public DateTime fechainicioSeleccion { get; set; }
            public DateTime fechaLimiteRetiro { get; set; }
            public DateTime fechafinSeleccion { get; set; }
        }
        public object Post(PeriodoDTO @newPeriodo)
        {
            using (var context = new AcademicSystemContext())
            {
                var incompletos = context.Periodos.Where(p => p.Status == SchemaTypes.PeriodStatus.En_Curso).ToList();
                if (incompletos.Count > 0)
                    foreach (var periodo in incompletos)
                    {
                        periodo.Status = SchemaTypes.PeriodStatus.Completado;
                    }
                var newP = new Periodo
                {
                    fechaFin = newPeriodo.fechaFin,
                    fechaInicio = newPeriodo.fechaInicio,
                    fechaInicioPreselecion = newPeriodo.fechaInicioPreselecion,
                    fechafinPreseleccion = newPeriodo.fechafinPreseleccion,
                    fechainicioSeleccion = newPeriodo.fechainicioSeleccion,
                    fechaLimiteRetiro = newPeriodo.fechaLimiteRetiro,
                    fechafinSeleccion = newPeriodo.fechafinSeleccion,
                    Status = SchemaTypes.PeriodStatus.En_Curso
                };
                context.Periodos.Add(newP);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, newP.PeriodoID);
            }

        }
        [HttpPut]
        public object conclude(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                context.Periodos
                       .Where(p => p.PeriodoID == id).FirstOrDefault()
                       .Status = SchemaTypes.PeriodStatus.Completado;
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }

        }

        [HttpGet]
        public object getPeriodDetails(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                var pdata = context.Periodos.Where(p => p.PeriodoID == id)
                                    .Select(d => new
                                    {
                                        d.fechaInicio,
                                        d.fechaFin,
                                        asignaturasRegistradas = d.periodAsignatures
                                                                   .Select(pa => new
                                                                   {
                                                                       nombreAsignatura = pa.Asignatura.Name,
                                                                       codigoAsignatura = pa.Asignatura.Codigo,
                                                                       profesor = pa.Profesor.Name + " " + pa.Profesor.LastName,
                                                                       //aula = pa.Aula.Building + "-" + pa.Aula.RoomID,
                                                                       seccion = pa.seccion,
                                                                       seccionID = pa.PeriodAsignatureID
                                                                       //cupos = pa.StudentsCount + "/" + pa.Aula.Asientos
                                                                   }).ToList()

                                    }).FirstOrDefault();
                return pdata;
            }
        }
        [HttpGet]
        public object getPeriods(string Query)
        {
            using (var context = new AcademicSystemContext())
            {
                var ResultSet = context.Periodos
                    .Where(p => p.fechaInicio.ToString().Contains(Query) || p.fechaFin.ToString().Contains(Query))
                    .Select(d => new
                    {
                        d.PeriodoID,
                        d.fechaInicio,
                        d.fechaFin
                    })
                    .ToList();
                return ResultSet;
            }
        }
        // PUT: api/Periodos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Periodos/5
        public void Delete(int id)
        {
        }
    }
}
