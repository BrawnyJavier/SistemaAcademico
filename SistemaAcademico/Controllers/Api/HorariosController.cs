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
    public class HorariosController : ApiController
    {
        // GET: api/Horarios
        public IEnumerable<object> Get()
        {
            using (var context = new DataModel.AcademicSystemContext())
            {
                return context.Horarios.ToList();
            }
        }

        // GET: api/Horarios/5
        public object GetSeccionHorarios(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                var d = context.Horarios
                               .Where(h => h.Asignatura.PeriodAsignatureID == id)
                               .Select(a =>
                                        new
                                        {
                                            a.HourInit,
                                            a.HourUntil,
                                            aula = a.Aula.RoomID,
                                            aulaName = a.Aula.Building + "-" + a.Aula.RoomNumber.ToString(),
                                            day = a.Day,
                                            id = a.HorarioID

                                        })
                                        .ToList();
                return d;
            }
        }

        public class HorarioDTO
        {
            public int roomID { get; set; }
            public int day { get; set; }
            public DateTime FechaInit { get; set; }
            public DateTime FechaFin { get; set; }
            public int PeriodAsigID { get; set; }
        }
        public object Post(HorarioDTO @new)
        {
            try
            {
                using (var context = new AcademicSystemContext())
                {
                    context.Horarios.Add(new Horario
                    {
                        Asignatura = context.PeriodAsignature
                                            .Where(a => a.PeriodAsignatureID == @new.PeriodAsigID)
                                            .FirstOrDefault(),
                        Aula = context.Rooms.Where(r => r.RoomID == @new.roomID).FirstOrDefault(),
                        Day = @new.day,
                        HourInit = @new.FechaInit,
                        HourUntil = @new.FechaFin
                    });
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        // PUT: api/Horarios/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        public HttpResponseMessage DeleteHorario(int id)
        {
            try
            {
                using (var context = new AcademicSystemContext())
                {
                    var hInDb = context.Horarios.Where(h => h.HorarioID == id).FirstOrDefault();
                    if (hInDb != null)
                    {
                        context.Horarios.Remove(hInDb);
                        context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
