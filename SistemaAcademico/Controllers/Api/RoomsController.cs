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
    public class RoomsController : ApiController
    {
        // GET: api/Rooms
        public IEnumerable<object> Get()
        {
            using (var context = new AcademicSystemContext())
            {
                return context.Rooms.ToList();
            }
        }

        // GET: api/Rooms/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Rooms
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Rooms/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Rooms/5
        public void Delete(int id)
        {
        }
    }
}
