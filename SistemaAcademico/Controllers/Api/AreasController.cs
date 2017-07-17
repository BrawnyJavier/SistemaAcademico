using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SistemaAcademico.Controllers.Api
{
    public class AreasController : ApiController
    {
        public IEnumerable<Classes.Area> getAreas()
        {
            using (var c = new SistemaAcademico.DataModel.AcademicSystemContext())
            {
                return c.Areas.ToList();
            }
        }
        public class AreaDTO
        {
            public int? areaID { get; set; }
            public string Name { get; set; }
            public string Codigo { get; set; }
        }
        public HttpResponseMessage createArea(AreaDTO data)
        {
            using (var c = new SistemaAcademico.DataModel.AcademicSystemContext())
            {
                c.Areas.Add(new Classes.Area
                {
                    Name = data.Name,
                    Codigo = data.Codigo
                });
                c.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created);
            }

        }
        [HttpPut]
        public HttpResponseMessage UpdateArea(AreaDTO data)
        {
            using (var c = new SistemaAcademico.DataModel.AcademicSystemContext())
            {
                var inDB = c.Areas.Where(a => a.AreaID == data.areaID).FirstOrDefault();
                if (inDB != null)
                {
                    inDB.Codigo = data.Codigo;
                    inDB.Name = data.Name;
                    c.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

        }
    }
}
