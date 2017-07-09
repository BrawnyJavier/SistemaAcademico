using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SistemaAcademico.Models;
using SistemaAcademico.DataModel;
using SistemaAcademico.DTOS;
using SistemaAcademico.Classes;

namespace SistemaAcademico.Controllers.Api
{
    public class MenuOptionsController : ApiController
    {
        private AcademicSystemContext context;

        public MenuOptionsController()
        {
            context = new DataModel.AcademicSystemContext();
        }

        // GET api/<controller>
        public IEnumerable<object> Get()
        {
            using (var context = new SistemaAcademico.DataModel.AcademicSystemContext())
            {
                ViewModels.IndexViewModel Model = new ViewModels.IndexViewModel();
                List<MenuOptionsDTO> MenuResults = new List<MenuOptionsDTO>();
                var source = context.OpcionesDelMenu.Where(o => o.parent == null).OrderBy(o => o.order).ToList();
                foreach (MenuOption Opcion in source)
                {
                    MenuResults.Add(new MenuOptionsDTO
                    {
                        OpcionDeMenu = Opcion,
                        Childs = context.OpcionesDelMenu
                                 .Where(child => child.parent.id == Opcion.id)
                                 .OrderBy(o => o.order)
                                 .ToList()
                    });
                }
                return MenuResults;
            }
        }

        // POST api/<controller>
        public object Post(MenuOption MenuOption)
        {
            if (ModelState.IsValid)
            {
                context.OpcionesDelMenu.Add(MenuOption);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        // PUT api/<controller>/5
        public object Put(MenuOption menuoption)
        {
            var current = context.OpcionesDelMenu.Where(m => m.id == menuoption.id).FirstOrDefault();
            current.Icon = menuoption.Icon;
            current.Link = menuoption.Link;
            current.Title = menuoption.Title;
            current.Description = menuoption.Description; context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/<controller>/5
        public object Delete(int id)
        {
            try
            {
                context.OpcionesDelMenu.Remove(context.OpcionesDelMenu.Where(m => m.id == id).First());
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                if (e.InnerException != null) return Request.CreateResponse(HttpStatusCode.InternalServerError);
                else return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }
    }
}