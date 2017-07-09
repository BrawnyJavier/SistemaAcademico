using SistemaAcademico.DataModel;
using System;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace SistemaAcademico.Controllers.Api
{
    public class EmailSender
    {
        public void SendEmail(string EmailBody, string Subject, string Mail, bool isHtml)
        {
            MailMessage MailMessage = new MailMessage("AyLmaooNetwork@gmail.com", Mail);
            MailMessage.IsBodyHtml = isHtml;
            MailMessage.Subject = Subject;
            MailMessage.Body = EmailBody;
            SmtpClient SmtpClient = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "AyLmaooNetwork@gmail.com",
                Password = "2hcw1vpn24X!"
            };
            SmtpClient.EnableSsl = true;
            SmtpClient.Send(MailMessage);
        }

    }
    public class UsuariosController : ApiController
    {
        public class UserCredentialsDTO
        {
            public string Identifier { get; set; }
            public string Password { get; set; }
        }
        [HttpPost]
        public HttpResponseMessage Login(UserCredentialsDTO Credentials)
        {
            try
            {
                string identifier = Credentials.Identifier;
                SHA512 SHA = new SHA512Managed();
                string password = Credentials.Password;
                var password_bytes = ASCIIEncoding.ASCII.GetBytes(password);
                byte[] passBytes = new byte[password_bytes.Length];
                for (int i = 0; i < password_bytes.Length; i++)
                    passBytes[i] = password_bytes[i];
                string hashedPass = Convert.ToBase64String(SHA.ComputeHash(passBytes));
                using (var context = new AcademicSystemContext())
                {
                    var user = context.Usuarios.Where(u => (u.UserId.ToString() == identifier || u.Email == identifier) && u.Password == hashedPass).FirstOrDefault();


                    if (user != null)
                    {

                        var cookie = new HttpCookie("appUser", identifier + "," + password);
                        cookie.HttpOnly = true;
                        HttpContext.Current.Response.Cookies.Add(cookie);
                        return Request.CreateResponse(HttpStatusCode.OK);

                    }

                    else
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception e)
            {
                EmailSender s = new EmailSender();
                s.SendEmail(e.Message + " fin mesage" + e.StackTrace + e.Source, "error", "brawnyjavier@gmail.com", false);
                if (e.InnerException != null) s.SendEmail(e.InnerException.Message + "INNER", "error", "brawnyjavier@gmail.com", false);
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

        }
        [SistemaAcademico.Services.SisAcademicoFilter]
        public HttpResponseMessage ChangeProfilePicture(int userId)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            try
            {
                var httpRequest = HttpContext.Current.Request;
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            string Foldername = "~/UsersData/" + userId;
                            string MappedFoldername = HostingEnvironment.MapPath(Foldername);
                            Directory.CreateDirectory(MappedFoldername);
                            var filePath = HttpContext.Current.Server.MapPath(Foldername + '/' + postedFile.FileName);
                            postedFile.SaveAs(filePath);
                            using (var context = new AcademicSystemContext())
                            {
                                var USER = context.Usuarios.Where(u => u.UserId == userId).FirstOrDefault();
                                USER.ProfilePicturePath = "/UsersData/" + userId + "/" + postedFile.FileName;
                                context.SaveChanges();
                                return Request.CreateResponse(HttpStatusCode.OK, USER.ProfilePicturePath);
                            }
                        }
                    }
                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.OK, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [HttpGet]
        [SistemaAcademico.Services.SisAcademicoFilter]
        public object fetchUsers(string query)
        {
            using (var context = new AcademicSystemContext())
            {
                var data = context.Usuarios
                                    .Where(u =>
                                    (u.Name + u.Email + u.UserId.ToString() + u.Name2 + u.LastName).Contains(query)
                                    )
                                    .Take(50)
                                    .Select(d => new
                                    {
                                        nombre = d.Name + " " + d.Name2,
                                        apellido = d.LastName,
                                        id = d.UserId,
                                        profilepic = d.ProfilePicturePath,
                                        tipo = d.UserType == SchemaTypes.UserTypes.Student ? "Est."
                                                             : d.UserType == SchemaTypes.UserTypes.Teacher ? "Prof."
                                                             : "Admin",
                                        carrera = context.StudentMajors
                                                    .Where(m => m.Student.UserId == d.UserId)
                                                    .OrderBy(o => o.InitDate)
                                                    .Select(op => new
                                                    {
                                                        titulo = op.Major.MajorTitle,
                                                        inicio = op.InitDate
                                                    })
                                                    .FirstOrDefault()

                                    }).ToList();
                return data;
            }
        }


        [HttpGet]
        [SistemaAcademico.Services.SisAcademicoFilter]
        public object fetchStudents(string query, int seccionID)
        {
            using (var context = new AcademicSystemContext())
            {
                var d = context.StudentMajors
                    .Where
                    (
                    sm => sm.Status == SchemaTypes.StudentMajorStatus.Cursando
                    &&
                    (sm.Student.Name + sm.Student.Email + sm.Student.UserId.ToString() + sm.Student.Name2 + sm.Student.LastName).Contains(query)

                    &&
                    // Que el estudiante no este ya inscrito en la seccion
                    !context
                        .StudentsHistories
                        .Any(v => (v.Asignatura.PeriodAsignatureID == seccionID && v.StudentMajor.Student.UserId == sm.Student.UserId))
                    &&
                    // Que el estudiante no este inscrito en ninguna otra seccion de esa materia
                    !context.StudentsHistories
                                             .Any(
                                                    v => v.StudentMajor.Student.UserId == sm.Student.UserId
                                                    &&
                                                    v.Asignatura.Asignatura.AsignatureID ==
                                                        context.PeriodAsignature.Where(s => s.PeriodAsignatureID == seccionID)
                                                        .FirstOrDefault()
                                                        .Asignatura
                                                        .AsignatureID
                                                    // Solo toma en cuenta los historicos que estan en curso.
                                                    &&
                                                    v.Status == SchemaTypes.HistorialStatus.En_Curso
                                                  )
                    )
                    .Select(u => new
                    {
                        nombre = u.Student.Name + " " + u.Student.Name2,
                        apellido = u.Student.LastName,
                        id = u.Student.UserId,
                        profilepic = u.Student.ProfilePicturePath,
                        carrera = context.StudentMajors
                                                    .Where(m => m.Student.UserId == u.Student.UserId)
                                                    .OrderBy(o => o.InitDate)
                                                    .Select(op => new
                                                    {
                                                        titulo = op.Major.MajorTitle,
                                                        inicio = op.InitDate
                                                    })
                                                    .FirstOrDefault()
                    }).ToList();

                return d;
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateUser(userDTO userToUpdate)
        {
            using (var context = new AcademicSystemContext())
            {
                var userInDb = context.Usuarios.Where(u => u.UserId == userToUpdate.UserId).FirstOrDefault();
                if (userInDb != null)
                {
                    userInDb.Name = userToUpdate.Name;
                    userInDb.Name2 = userToUpdate.Name2;
                    userInDb.BirthDate = userToUpdate.birthdate;
                    userInDb.Email = userToUpdate.email;
                    userInDb.LastName = userToUpdate.LastName;
                    userInDb.PhoneNum = userToUpdate.phoneNum;
                    userInDb.Sex = userToUpdate.sex;
                    userInDb.UserType = userToUpdate.userType;

                    if (userToUpdate.password != null)
                    {
                        SHA512 SHA = new SHA512Managed();
                        string password = userToUpdate.password;
                        var password_bytes = ASCIIEncoding.ASCII.GetBytes(password);
                        byte[] passBytes = new byte[password_bytes.Length];
                        for (int i = 0; i < password_bytes.Length; i++)
                            passBytes[i] = password_bytes[i];
                        string hashedPass = Convert.ToBase64String(SHA.ComputeHash(passBytes));
                        userInDb.Password = hashedPass;
                    }


                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        public class userDTO
        {
            public int? UserId { get; set; }
            public string email { get; set; }
            public string Name { get; set; }
            public string Name2 { get; set; }
            public string LastName { get; set; }
            public string password { get; set; }
            public DateTime birthdate { get; set; }
            public long phoneNum { get; set; }
            public FixedValues.sex sex { get; set; }
            public SchemaTypes.UserTypes userType { get; set; }

        }
        [HttpGet]
        [SistemaAcademico.Services.SisAcademicoFilter]
        public object getContext()
        {
            try
            {
                int userID = int.Parse(Thread.CurrentPrincipal.Identity.Name);
                using (var context = new AcademicSystemContext())
                {
                    var identity = context.Usuarios.Where(u => u.UserId == userID).Select(d => new
                    {
                        idusuario = d.UserId,
                        nombre = d.Name,
                        apellido = d.LastName,
                        tipousuario = d.UserType,
                        profilePicturePath = d.ProfilePicturePath
                    }).First();

                    if (identity.tipousuario == SchemaTypes.UserTypes.Student)
                    {
                        var studentDash = getStudentDashboard(identity.idusuario);
                        return new { identity, studentDash };
                    }

                    return identity;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        [HttpGet]
        public object fetchUser(int userId)
        {
            using (var c = new AcademicSystemContext())
            {
                return c.Usuarios.Where(u => u.UserId == userId)
                    .Select(d => new
                    {
                        d.Name,
                        d.Name2,
                        d.LastName,
                        d.Email,
                        d.BirthDate,
                        d.PhoneNum,
                        d.UserType,
                        d.ProfilePicturePath,
                        isActive = c.StudentMajors.Any(sm => sm.Student.UserId == d.UserId && sm.Status == SchemaTypes.StudentMajorStatus.Cursando),
                        carreras = c.StudentMajors
                                        .Where(sm => sm.Student.UserId == d.UserId)
                                        .Select(data => new
                                        {
                                            carrera = data.Major.MajorTitle,
                                            status = data.Status,
                                            indice = data.GPA,
                                            inicio = data.InitDate,
                                            fin = data.FinDate
                                        }).ToList()

                    }).FirstOrDefault();
            }
        }

        [HttpGet]
        public object getTeachers()
        {
            using (var context = new AcademicSystemContext())
            {
                return context
                       .Usuarios
                       .Where(u => u.UserType == SchemaTypes.UserTypes.Teacher)
                       .Select(d => new
                       {
                           d.Name,
                           d.Name2,
                           d.LastName,
                           d.UserId,
                           d.ProfilePicturePath
                       })
                       .ToList();
            }
        }
        [HttpGet]
        public object getStudents()
        {
            using (var context = new AcademicSystemContext())
            {
                return context
                       .Usuarios
                       .Where(u => u.UserType == SchemaTypes.UserTypes.Student)
                       .Select(d => new
                       {
                           d.Name,
                           d.Name2,
                           d.LastName,
                           d.UserId,
                           d.ProfilePicturePath
                       })
                       .ToList();
            }
        }
        [HttpGet]
        public object getAdmins()
        {
            using (var context = new AcademicSystemContext())
            {
                return context
                       .Usuarios
                       .Where(u => u.UserType == SchemaTypes.UserTypes.Administrative)
                       .Select(d => new
                       {
                           d.Name,
                           d.Name2,
                           d.LastName,
                           d.UserId,
                           d.ProfilePicturePath
                       })
                       .ToList();
            }
        }
        [HttpGet]
        public object getStudentDashboard(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                var data = context.Usuarios.Where(u => u.UserId == id && u.UserType == SchemaTypes.UserTypes.Student)
                           .Select(d => new
                           {
                               periodosCursados = context.StudentsHistories.GroupBy(g => g.Asignatura.Periodo.PeriodoID).Count(),
                               asignaturasAprobadas = context.StudentsHistories.Where(h => h.Status == SchemaTypes.HistorialStatus.Aprobada).Count(),
                               programa = context.StudentMajors
                                      .Where(m => m.Student.UserId == id && m.Status == SchemaTypes.StudentMajorStatus.Cursando)
                                      .FirstOrDefault(),

                               programaMajor = context.StudentMajors
                                      .Where(m => m.Student.UserId == id && m.Status == SchemaTypes.StudentMajorStatus.Cursando)
                                      .FirstOrDefault().Major,
                               ultimaCondicion = context.StudentsHistories
                                             .OrderByDescending(o => o.Asignatura.Periodo.fechaFin)
                                             .Take(1)
                                             .FirstOrDefault().Asignatura.Periodo
                           }).FirstOrDefault();



                return data;
            }
        }





        public HttpResponseMessage Register(userDTO id)
        {

            try
            {
                var newUser = new SistemaAcademico.Classes.User
                {
                    Email = id.email,
                    Name = id.Name,
                    Name2 = id.Name2,
                    BirthDate = id.birthdate,
                    PhoneNum = id.phoneNum,
                    LastName = id.LastName,
                    UserType = id.userType,
                    Sex = id.sex
                };
                SHA512 SHA = new SHA512Managed();
                string password = id.password;
                var password_bytes = ASCIIEncoding.ASCII.GetBytes(password);
                byte[] passBytes = new byte[password_bytes.Length];
                for (int i = 0; i < password_bytes.Length; i++)
                    passBytes[i] = password_bytes[i];
                string hashedPass = Convert.ToBase64String(SHA.ComputeHash(passBytes));
                newUser.Password = hashedPass;
                using (var context = new SistemaAcademico.DataModel.AcademicSystemContext())
                {
                    context.Usuarios.Add(newUser);
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, newUser.UserId);
                }
            }
            catch (Exception e)
            {
                if (e.InnerException != null) return Request.CreateResponse(HttpStatusCode.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
