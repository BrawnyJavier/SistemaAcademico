using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Threading;
using System.Security.Principal;
using SistemaAcademico.DataModel;
using System.Security.Cryptography;
using System.Net;

namespace SistemaAcademico.Services
{
    /*
    * AUTHOR: Brawny Javier Mateo Reyes
    * DATE: 24/6/2017 
    * DESCRIPTION : Filtro aplicado para limitar acceso a los recursos del sistema a quienes esten logueados.
    */
    public class SisAcademicoFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                string id = actionContext.Request.Headers.GetCookies("appUser").First()["appUser"].Value;
                string[] identity = id.Split(',');
                string identifier = identity[0];
                SHA512 SHA = new SHA512Managed();
                string password = identity[1];
                var password_bytes = ASCIIEncoding.ASCII.GetBytes(password);
                byte[] passBytes = new byte[password_bytes.Length];
                for (int i = 0; i < password_bytes.Length; i++)
                    passBytes[i] = password_bytes[i];
                string hashedPass = Convert.ToBase64String(SHA.ComputeHash(passBytes));
                using (var context = new AcademicSystemContext())
                {
                    var user = context.Usuarios.Where(u => (u.UserId.ToString() == identifier || u.Email == identifier) && u.Password == hashedPass).FirstOrDefault();
                    if (user != null)
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(user.UserId.ToString()), null);

                    else
                        actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception)
            {
                actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }


        }
    }
}