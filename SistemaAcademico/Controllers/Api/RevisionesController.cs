using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SistemaAcademico.DataModel;
using SistemaAcademico.Classes;
using SistemaAcademico.Services;
using System.Threading;


namespace SistemaAcademico.Controllers.Api
{
    [Services.SisAcademicoFilter]
    public class RevisionesController : ApiController
    {
        public class solicitudDTO
        {

            public int historialID { get; set; }
            public string motivo { get; set; }
        }
        [HttpPost]
        public object solicitarRevision(solicitudDTO val)
        {
            using (var context = new AcademicSystemContext())
            {
                context.SolicitudesRevisiones.Add(new solicitudRevision
                {
                    estado = SchemaTypes.revisionStatus.EnEspera,
                    motivoSolicitud = val.motivo,
                    historial = context.StudentsHistories.Where(h => h.HistoryLineId == val.historialID).FirstOrDefault(),
                    fechaSolicitud = DateTime.Now

                });

                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        public class DenegationDTO
        {
            public int solicitudID { get; set; }
            public string TeacherComment { get; set; }
        }
        [HttpPut]
        public HttpResponseMessage denegateSolicitud(DenegationDTO denegation)
        {
            using (var context = new AcademicSystemContext())
            {
                var solinDb = context.SolicitudesRevisiones.Where(s => s.SolRevisionID == denegation.solicitudID).FirstOrDefault();
                solinDb.estado = SchemaTypes.revisionStatus.NoAprobadaProfesor;
                solinDb.ComentarioProfesor = denegation.TeacherComment;
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        [HttpGet]
        public object getRevisionDetails(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                return context.SolicitudesRevisiones
                              .Where(s => s.SolRevisionID == id).Select(d => new
                              {
                                  motivo = d.motivoSolicitud,
                                  comentariosProfesor = d.ComentarioProfesor,
                                  califAnterior = d.calificacionAnterior,
                                  //reunionLugar = d.historial.Asignatura.Asignatura.
                                  comentarioAdmin = d.ComentarioAdmin,
                                  fechaReunion = d.fechaReunion,
                                  califActual = context.StudentsHistories
                                                       .Where(h => h.HistoryLineId == d.historial.HistoryLineId)
                                                       .Select(c => c.Calificacion)
                                                       .FirstOrDefault()
                              }).FirstOrDefault();
            }
        }
        public class citaDTO
        {
            public int solRevisionID { get; set; }
            public DateTime fechaReunion { get; set; }
            public string comentarios { get; set; }
        }
        [HttpPost]
        public HttpResponseMessage generateReunion(citaDTO cita)
        {
            using (var context = new AcademicSystemContext())
            {
                int currserID = int.Parse(Thread.CurrentPrincipal.Identity.Name);
                var solicitud = context.SolicitudesRevisiones.Where(i => i.SolRevisionID == cita.solRevisionID).FirstOrDefault();
                if (solicitud != null)
                {
                    solicitud.estado = SchemaTypes.revisionStatus.Citada;
                    solicitud.fechaReunion = cita.fechaReunion;
                    solicitud.ComentarioProfesor = cita.comentarios;
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
