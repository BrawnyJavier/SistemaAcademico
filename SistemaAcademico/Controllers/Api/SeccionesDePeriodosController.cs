using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SistemaAcademico.DataModel;
using SistemaAcademico.Classes;
using System.Threading;

namespace SistemaAcademico.Controllers.Api
{
    public class SeccionesDePeriodosController : ApiController
    {

        // GET: api/SeccionesDePeriodos/5
        public IEnumerable<object> Get(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                return context
                            .PeriodAsignature
                            .Where(p => p.Periodo.PeriodoID == id)
                            .ToList();
            }
        }

        public class PeriodAsignatureDTO
        {
            public int PeriodID { get; set; }
            public int AsignatureID { get; set; }
            public int TeacherID { get; set; }
            public int SeccionNumber { get; set; }
        }
        [HttpPost]
        public void PostSeccion(PeriodAsignatureDTO @new)
        {
            using (var context = new AcademicSystemContext())
            {
                context.PeriodAsignature.Add(new PeriodAsignature
                {
                    Asignatura = context
                                .Asignaturas
                                .Where(a => a.AsignatureID == @new.AsignatureID)
                                .FirstOrDefault(),
                    Profesor = context
                                .Usuarios
                                .Where(t => t.UserId == @new.TeacherID && t.UserType == SchemaTypes.UserTypes.Teacher)
                                .FirstOrDefault(),
                    Periodo = context.Periodos.Where(p => p.PeriodoID == @new.PeriodID).FirstOrDefault(),
                    seccion = context
                                .PeriodAsignature
                                .Where(a => a.Asignatura.AsignatureID == @new.AsignatureID && a.Periodo.PeriodoID == @new.PeriodID)
                                .Count() + 1,
                    Status = SchemaTypes.PeriodAsignatureStatus.Creada

                });
                context.SaveChanges();
            }
        }
        [HttpGet]
        [Services.SisAcademicoFilter]
        public object getAsignaturasPublicadas()
        {
            int userId = int.Parse(Thread.CurrentPrincipal.Identity.Name);

            using (var context = new AcademicSystemContext())
            {
                int currPEriodID = context.Periodos.Where(P => P.Status == SchemaTypes.PeriodStatus.En_Curso).First().PeriodoID;

                var pubs = context.StudentsHistories
                    .Where(h => h.StudentMajor.Student.UserId == userId
                                && h.Asignatura.Periodo.PeriodoID == currPEriodID
                                && h.Calificacion != 0)
                    .Select(d => new
                    {
                        agendada = context.SolicitudesRevisiones
                        .Any(w => w.historial.HistoryLineId == d.HistoryLineId) ? context.SolicitudesRevisiones
                                                                                     .Where(h => h.historial.HistoryLineId == d.HistoryLineId)
                                                                                     .FirstOrDefault()
                                                                                     .estado
                                                                                     .ToString() : "404",
                        nota = d.Calificacion,
                        historialId = d.HistoryLineId,
                        alpha = d.CalificacionAlpha,
                        materiaCodigo = d.Asignatura.Asignatura.Codigo,
                        materianame = d.Asignatura.Asignatura.Name,
                        profesor = d.Asignatura.Profesor.Name + " " + d.Asignatura.Profesor.LastName,
                        solicitudRevisionId = context.SolicitudesRevisiones.Where(w => w.historial.HistoryLineId == d.HistoryLineId).Select(s => s.SolRevisionID).FirstOrDefault()
                    })
                    .ToList();
                return pubs;


            }
        }



        [HttpGet]
        public object getSection(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                var QuerySet = context.PeriodAsignature
                    .Where(p => p.PeriodAsignatureID == id)
                    .Select(D => new
                    {
                        PeriodoID = D.Periodo.PeriodoID,
                        fechaInicio = D.Periodo.fechaInicio,
                        fechaFin = D.Periodo.fechaFin,

                        seccion = D.seccion,
                        profesor = (D.Profesor == null) ? "pendiente" : D.Profesor.Name + " " + D.Profesor.Name2 + " " + D.Profesor.LastName,
                        profesorID = (D.Profesor == null) ? "pendiente" : D.Profesor.UserId.ToString(),
                        Asignatura = D.Asignatura.Codigo,
                        AsignaturaName = D.Asignatura.Name,

                    })

                    .OrderBy(a => a.Asignatura)
                    .OrderBy(a => a.seccion)
                    .Select(D => new
                    {
                        Periodo = D.PeriodoID,
                        PInicio = D.fechaInicio,
                        PFin = D.fechaFin,
                        seccion = (D.seccion < 10) ? "0" + D.seccion.ToString() : D.seccion.ToString(),
                        profesor = D.profesor,
                        profesorID = D.profesorID,
                        Asignatura = D.Asignatura,

                        AsignaturaName = D.AsignaturaName
                    }).ToList();

                var Horarios = context.Horarios.Where(H => H.Asignatura.PeriodAsignatureID == id).ToList();

                var EstudiantesInscritos = context
                    .StudentsHistories
                    .Where(s => s.Asignatura.PeriodAsignatureID == id)
                    .Select(d => new
                    {
                        NombreEst = d.StudentMajor.Student.Name + " " + d.StudentMajor.Student.Name2 + " " + d.StudentMajor.Student.LastName,
                        IdEst = d.StudentMajor.Student.UserId,
                        FotoEst = d.StudentMajor.Student.ProfilePicturePath,
                        CarreraEst = d.StudentMajor.Major.MajorTitle,
                        idHistorial = d.HistoryLineId

                    })
                    .ToList();

                var data = new
                {
                    QuerySet,
                    Horarios,
                    EstudiantesInscritos
                };

                return data;
            }
        }
        [HttpGet]
        public object getStudents(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                var EstudiantesInscritos = context
                .StudentsHistories
                .Where(s => s.Asignatura.PeriodAsignatureID == id)
                .Select(d => new
                {
                    NombreEst = d.StudentMajor.Student.Name + " " + d.StudentMajor.Student.Name2 + " " + d.StudentMajor.Student.LastName,
                    IdEst = d.StudentMajor.Student.UserId,
                    FotoEst = d.StudentMajor.Student.ProfilePicturePath,
                    CarreraEst = d.StudentMajor.Major.MajorTitle,
                    idHistorial = d.HistoryLineId,
                    calificado = d.Calificacion > 0 ? true : false

                })
                .ToList();
                return EstudiantesInscritos;
            }
        }

        public class InscripcionDTO
        {
            public int periodSeccionID { get; set; }
            public int estudentID { get; set; }
        }
        public object inscribirEst(InscripcionDTO @new)
        {
            using (var context = new AcademicSystemContext())
            {
                PeriodAsignature sec = context.PeriodAsignature.Where(p => p.PeriodAsignatureID == @new.periodSeccionID).FirstOrDefault();
                context.StudentsHistories.Add(new StudentHistory
                {
                    Asignatura = sec,
                    StudentMajor = context.StudentMajors.Where(s => s.Student.UserId == @new.estudentID && s.Status == SchemaTypes.StudentMajorStatus.Cursando).First(),
                    Status = SchemaTypes.HistorialStatus.En_Curso
                });
                context.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }
        // PUT: api/SeccionesDePeriodos/5
        [HttpPut]
        public object BorrarEstudiante(int id)
        {
            using (var context = new AcademicSystemContext())
            {
                var SH = context.StudentsHistories
                      .Where(sh => sh.HistoryLineId == id)
                      .FirstOrDefault();

                context.StudentsHistories.Remove(SH);
                context.SaveChanges();
                return HttpStatusCode.OK;
            }
        }
        [HttpGet]
        [SistemaAcademico.Services.SisAcademicoFilter]
        public object getTeachersSections()
        {
            var currUser = int.Parse(Thread.CurrentPrincipal.Identity.Name);

            using (var context = new AcademicSystemContext())
            {
                var currentPeriod = context.Periodos.Where(p => p.Status == SchemaTypes.PeriodStatus.En_Curso).First().PeriodoID;

                var teacherAsigs = context.PeriodAsignature.Where(p => p.Periodo.PeriodoID == currentPeriod && p.Profesor.UserId == currUser)
                    .Select(d => new
                    {
                        idSec = d.PeriodAsignatureID,
                        seccion = d.seccion,
                        asignatura = new { nombre = d.Asignatura.Name, id = d.Asignatura.AsignatureID }

                    }).ToList();
                return teacherAsigs;
            }
        }
        [HttpPost]
        public object FillDB()
        {
            string[] ppName = { "Abbott", "Acevedo", "Acosta", "Adams", "Adkins", "Aguilar", "Aguirre", "Albert", "Alexander", "Alford", "Allen", "Allison", "Alston", "Alvarado", "Alvarez", "Anderson", "Andrews", "Anthony", "Armstrong", "Arnold", "Ashley", "Atkins", "Atkinson", "Austin", "Avery", "Avila", "Ayala", "Ayers", "Bailey", "Baird", "Baker", "Baldwin", "Ball", "Ballard", "Banks", "Barber", "Barker", "Barlow", "Barnes", "Barnett", "Barr", "Barrera", "Barrett", "Barron", "Barry", "Bartlett", "Barton", "Bass", "Bates", "Battle", "Bauer", "Baxter", "Beach", "Bean", "Beard", "Beasley", "Beck", "Becker", "Bell", "Bender", "Benjamin", "Bennett", "Benson", "Bentley", "Benton", "Berg", "Berger", "Bernard", "Berry", "Best", "Bird", "Bishop", "Black", "Blackburn", "Blackwell", "Blair", "Blake", "Blanchard", "Blankenship", "Blevins", "Bolton", "Bond", "Bonner", "Booker", "Boone", "Booth", "Bowen", "Bowers", "Bowman", "Boyd", "Boyer", "Boyle", "Bradford", "Bradley", "Bradshaw", "Brady", "Branch", "Bray", "Brennan", "Brewer", "Bridges", "Briggs", "Bright", "Britt", "Brock", "Brooks", "Brown", "Browning", "Bruce", "Bryan", "Bryant", "Buchanan", "Buck", "Buckley", "Buckner", "Bullock", "Burch", "Burgess", "Burke", "Burks", "Burnett", "Burns", "Burris", "Burt", "Burton", "Bush", "Butler", "Byers", "Byrd", "Cabrera", "Cain", "Calderon", "Caldwell", "Calhoun", "Callahan", "Camacho", "Cameron", "Campbell", "Campos", "Cannon", "Cantrell", "Cantu", "Cardenas", "Carey", "Carlson", "Carney", "Carpenter", "Carr", "Carrillo", "Carroll", "Carson", "Carter", "Carver", "Case", "Casey", "Cash", "Castaneda", "Castillo", "Castro", "Cervantes", "Chambers", "Chan", "Chandler", "Chaney", "Chang", "Chapman", "Charles", "Chase", "Chavez", "Chen", "Cherry", "Christensen", "Christian", "Church", "Clark", "Clarke", "Clay", "Clayton", "Clements", "Clemons", "Cleveland", "Cline", "Cobb", "Cochran", "Coffey", "Cohen", "Cole", "Coleman", "Collier", "Collins", "Colon", "Combs", "Compton", "Conley", "Conner", "Conrad", "Contreras", "Conway", "Cook", "Cooke", "Cooley", "Cooper", "Copeland", "Cortez", "Cote", "Cotton", "Cox", "Craft", "Craig", "Crane", "Crawford", "Crosby", "Cross", "Cruz", "Cummings", "Cunningham", "Curry", "Curtis", "Dale", "Dalton", "Daniel", "Daniels", "Daugherty", "Davenport", "David", "Davidson", "Davis", "Dawson", "Day", "Dean", "Decker", "Dejesus", "Delacruz", "Delaney", "Deleon", "Delgado", "Dennis", "Diaz", "Dickerson", "Dickson", "Dillard", "Dillon", "Dixon", "Dodson", "Dominguez", "Donaldson", "Donovan", "Dorsey", "Dotson", "Douglas", "Downs", "Doyle", "Drake", "Dudley", "Duffy", "Duke", "Duncan", "Dunlap", "Dunn", "Duran", "Durham", "Dyer", "Eaton", "Edwards", "Elliott", "Ellis", "Ellison", "Emerson", "England", "English", "Erickson", "Espinoza", "Estes", "Estrada", "Evans", "Everett", "Ewing", "Farley", "Farmer", "Farrell", "Faulkner", "Ferguson", "Fernandez", "Ferrell", "Fields", "Figueroa", "Finch", "Finley", "Fischer", "Fisher", "Fitzgerald", "Fitzpatrick", "Fleming", "Fletcher", "Flores", "Flowers", "Floyd", "Flynn", "Foley", "Forbes", "Ford", "Foreman", "Foster", "Fowler", "Fox", "Francis", "Franco", "Frank", "Franklin", "Franks", "Frazier", "Frederick", "Freeman", "French", "Frost", "Fry", "Frye", "Fuentes", "Fuller", "Fulton", "Gaines", "Gallagher", "Gallegos", "Galloway", "Gamble", "Garcia", "Gardner", "Garner", "Garrett", "Garrison", "Garza", "Gates", "Gay", "Gentry", "George", "Gibbs", "Gibson", "Gilbert", "Giles", "Gill", "Gillespie", "Gilliam", "Gilmore", "Glass", "Glenn", "Glover", "Goff", "Golden", "Gomez", "Gonzales", "Gonzalez", "Good", "Goodman", "Goodwin", "Gordon", "Gould", "Graham", "Grant", "Graves", "Gray", "Green", "Greene", "Greer", "Gregory", "Griffin", "Griffith", "Grimes", "Gross", "Guerra", "Guerrero", "Guthrie", "Gutierrez", "Guy", "Guzman", "Hahn", "Hale", "Haley", "Hall", "Hamilton", "Hammond", "Hampton", "Hancock", "Haney", "Hansen", "Hanson", "Hardin", "Harding", "Hardy", "Harmon", "Harper", "Harrell", "Harrington", "Harris", "Harrison", "Hart", "Hartman", "Harvey", "Hatfield", "Hawkins", "Hayden", "Hayes", "Haynes", "Hays", "Head", "Heath", "Hebert", "Henderson", "Hendricks", "Hendrix", "Henry", "Hensley", "Henson", "Herman", "Hernandez", "Herrera", "Herring", "Hess", "Hester", "Hewitt", "Hickman", "Hicks", "Higgins", "Hill", "Hines", "Hinton", "Hobbs", "Hodge", "Hodges", "Hoffman", "Hogan", "Holcomb", "Holden", "Holder", "Holland", "Holloway", "Holman", "Holmes", "Holt", "Hood", "Hooper", "Hoover", "Hopkins", "Hopper", "Horn", "Horne", "Horton", "House", "Houston", "Howard", "Howe", "Howell", "Hubbard", "Huber", "Hudson", "Huff", "Huffman", "Hughes", "Hull", "Humphrey", "Hunt", "Hunter", "Hurley", "Hurst", "Hutchinson", "Hyde", "Ingram", "Irwin", "Jackson", "Jacobs", "Jacobson", "James", "Jarvis", "Jefferson", "Jenkins", "Jennings", "Jensen", "Jimenez", "Johns", "Johnson", "Johnston", "Jones", "Jordan", "Joseph", "Joyce", "Joyner", "Juarez", "Justice", "Kane", "Kaufman", "Keith", "Keller", "Kelley", "Kelly", "Kemp", "Kennedy", "Kent", "Kerr", "Key", "Kidd", "Kim", "King", "Kinney", "Kirby", "Kirk", "Kirkland", "Klein", "Kline", "Knapp", "Knight", "Knowles", "Knox", "Koch", "Kramer", "Lamb", "Lambert", "Lancaster", "Landry", "Lane", "Lang", "Langley", "Lara", "Larsen", "Larson", "Lawrence", "Lawson", "Le", "Leach", "Leblanc", "Lee", "Leon", "Leonard", "Lester", "Levine", "Levy", "Lewis", "Lindsay", "Lindsey", "Little", "Livingston", "Lloyd", "Logan", "Long", "Lopez", "Lott", "Love", "Lowe", "Lowery", "Lucas", "Luna", "Lynch", "Lynn", "Lyons", "Macdonald", "Macias", "Mack", "Madden", "Maddox", "Maldonado", "Malone", "Mann", "Manning", "Marks", "Marquez", "Marsh", "Marshall", "Martin", "Martinez", "Mason", "Massey", "Mathews", "Mathis", "Matthews", "Maxwell", "May", "Mayer", "Maynard", "Mayo", "Mays", "Mcbride", "Mccall", "Mccarthy", "Mccarty", "Mcclain", "Mcclure", "Mcconnell", "Mccormick", "Mccoy", "Mccray", "Mccullough", "Mcdaniel", "Mcdonald", "Mcdowell", "Mcfadden", "Mcfarland", "Mcgee", "Mcgowan", "Mcguire", "Mcintosh", "Mcintyre", "Mckay", "Mckee", "Mckenzie", "Mckinney", "Mcknight", "Mclaughlin", "Mclean", "Mcleod", "Mcmahon", "Mcmillan", "Mcneil", "Mcpherson", "Meadows", "Medina", "Mejia", "Melendez", "Melton", "Mendez", "Mendoza", "Mercado", "Mercer", "Merrill", "Merritt", "Meyer", "Meyers", "Michael", "Middleton", "Miles", "Miller", "Mills", "Miranda", "Mitchell", "Molina", "Monroe", "Montgomery", "Montoya", "Moody", "Moon", "Mooney", "Moore", "Morales", "Moran", "Moreno", "Morgan", "Morin", "Morris", "Morrison", "Morrow", "Morse", "Morton", "Moses", "Mosley", "Moss", "Mueller", "Mullen", "Mullins", "Munoz", "Murphy", "Murray", "Myers", "Nash", "Navarro", "Neal", "Nelson", "Newman", "Newton", "Nguyen", "Nichols", "Nicholson", "Nielsen", "Nieves", "Nixon", "Noble", "Noel", "Nolan", "Norman", "Norris", "Norton", "Nunez", "Obrien", "Ochoa", "Oconnor", "Odom", "Odonnell", "Oliver", "Olsen", "Olson", "Oneal", "Oneil", "Oneill", "Orr", "Ortega", "Ortiz", "Osborn", "Osborne", "Owen", "Owens", "Pace", "Pacheco", "Padilla", "Page", "Palmer", "Park", "Parker", "Parks", "Parrish", "Parsons", "Pate", "Patel", "Patrick", "Patterson", "Patton", "Paul", "Payne", "Pearson", "Peck", "Pena", "Pennington", "Perez", "Perkins", "Perry", "Peters", "Petersen", "Peterson", "Petty", "Phelps", "Phillips", "Pickett", "Pierce", "Pittman", "Pitts", "Pollard", "Poole", "Pope", "Porter", "Potter", "Potts", "Powell", "Powers", "Pratt", "Preston", "Price", "Prince", "Pruitt", "Puckett", "Pugh", "Quinn", "Ramirez", "Ramos", "Ramsey", "Randall", "Randolph", "Rasmussen", "Ratliff", "Ray", "Raymond", "Reed", "Reese", "Reeves", "Reid", "Reilly", "Reyes", "Reynolds", "Rhodes", "Rice", "Rich", "Richard", "Richards", "Richardson", "Richmond", "Riddle", "Riggs", "Riley", "Rios", "Rivas", "Rivera", "Rivers", "Roach", "Robbins", "Roberson", "Roberts", "Robertson", "Robinson", "Robles", "Rocha", "Rodgers", "Rodriguez", "Rodriquez", "Rogers", "Rojas", "Rollins", "Roman", "Romero", "Rosa", "Rosales", "Rosario", "Rose", "Ross", "Roth", "Rowe", "Rowland", "Roy", "Ruiz", "Rush", "Russell", "Russo", "Rutledge", "Ryan", "Salas", "Salazar", "Salinas", "Sampson", "Sanchez", "Sanders", "Sandoval", "Sanford", "Santana", "Santiago", "Santos", "Sargent", "Saunders", "Savage", "Sawyer", "Schmidt", "Schneider", "Schroeder", "Schultz", "Schwartz", "Scott", "Sears", "Sellers", "Serrano", "Sexton", "Shaffer", "Shannon", "Sharp", "Sharpe", "Shaw", "Shelton", "Shepard", "Shepherd", "Sheppard", "Sherman", "Shields", "Short", "Silva", "Simmons", "Simon", "Simpson", "Sims", "Singleton", "Skinner", "Slater", "Sloan", "Small", "Smith", "Snider", "Snow", "Snyder", "Solis", "Solomon", "Sosa", "Soto", "Sparks", "Spears", "Spence", "Spencer", "Stafford", "Stanley", "Stanton", "Stark", "Steele", "Stein", "Stephens", "Stephenson", "Stevens", "Stevenson", "Stewart", "Stokes", "Stone", "Stout", "Strickland", "Strong", "Stuart", "Suarez", "Sullivan", "Summers", "Sutton", "Swanson", "Sweeney", "Sweet", "Sykes", "Talley", "Tanner", "Tate", "Taylor", "Terrell", "Terry", "Thomas", "Thompson", "Thornton", "Tillman", "Todd", "Torres", "Townsend", "Tran", "Travis", "Trevino", "Trujillo", "Tucker", "Turner", "Tyler", "Tyson", "Underwood", "Valdez", "Valencia", "Valentine", "Valenzuela", "Vance", "Vang", "Vargas", "Vasquez", "Vaughan", "Vaughn", "Vazquez", "Vega", "Velasquez", "Velazquez", "Velez", "Villarreal", "Vincent", "Vinson", "Wade", "Wagner", "Walker", "Wall", "Wallace", "Waller", "Walls", "Walsh", "Walter", "Walters", "Walton", "Ward", "Ware", "Warner", "Warren", "Washington", "Waters", "Watkins", "Watson", "Watts", "Weaver", "Webb", "Weber", "Webster", "Weeks", "Weiss", "Welch", "Wells", "West", "Wheeler", "Whitaker", "White", "Whitehead", "Whitfield", "Whitley", "Whitney", "Wiggins", "Wilcox", "Wilder", "Wiley", "Wilkerson", "Wilkins", "Wilkinson", "William", "Williams", "Williamson", "Willis", "Wilson", "Winters", "Wise", "Witt", "Wolf", "Wolfe", "Wong", "Wood", "Woodard", "Woods", "Woodward", "Wooten", "Workman", "Wright", "Wyatt", "Wynn", "Yang", "Yates", "York", "Young", "Zamora", "Zimmerman" };
            using (var context = new AcademicSystemContext())
            {
                for (int i = 0; i < 150000; i++)
                {
                    Random r = new Random();
                    int rInt = r.Next(0, ppName.Length); //for ints
                    int rInt2 = r.Next(0, ppName.Length); //for ints
                    int rInt3 = r.Next(0, ppName.Length); //for ints

                    context.Usuarios.Add(new Classes.User
                    {
                        Name = ppName[rInt],
                        BirthDate = DateTime.Now,
                        Sex = FixedValues.sex.Femenine,
                        Email = ppName[rInt] + i.ToString() + "04@intec.edu.do",
                        LastName = ppName[rInt2],
                        Name2 = ppName[rInt3],
                        PhoneNum = rInt + rInt2 + rInt3 + i,
                        UserType = SchemaTypes.UserTypes.Student,
                        ProfilePicturePath = "/UsersData/17/images.png",
                        Password = "x61Ey612Kl2gpFL56FT9weDnpSo4AV8j8+qx2AuTHdRyY036xxzTTrw10Wq3+4qQyB+XURPWx1ONxp3Y3pB37A=="

                    });

                    if (i == 5000 || i == 10000 || i == 10000 * 2 || i == 10000 * 3 || i == 10000 * 4 || i == 10000 * 5 || i == 10000 * 6 || i == 10000 * 7 || i == 10000 * 8 || i == 10000 * 9 || i == 10000 * 10)
                        context.SaveChanges();

                }
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        [HttpGet]
        [Services.SisAcademicoFilter]
        public object getTeachersRevisiones()
        {
            int userID = int.Parse(Thread.CurrentPrincipal.Identity.Name);

            using (var context = new AcademicSystemContext())
            {

                //var ta = (from historial in context.StudentsHistories
                //          join solicitudes in context.SolicitudesRevisiones
                //          on historial.HistoryLineId equals solicitudes.historial.HistoryLineId
                //          where historial.
                //          select new
                //          {
                //              idSolicitud = solicitudes.SolRevisionID,
                //              historyLineId = historial.HistoryLineId
                //          }).ToList();





                var data = context.SolicitudesRevisiones
                           .Where(h => h.historial.Asignatura.Profesor.UserId == userID
                                        && h.estado == SchemaTypes.revisionStatus.EnEspera)
                           .Select(n => new
                           {
                               idSolicitud = n.SolRevisionID,
                               historyLine = n.historial.HistoryLineId,
                               status = n.estado,
                               motivo = n.motivoSolicitud,
                               fechaSolicitud = n.fechaSolicitud,
                               seccion = n.historial.Asignatura.seccion > 10 ? n.historial.Asignatura.seccion.ToString() : 0.ToString() + n.historial.Asignatura.seccion.ToString(),
                               estudiante = n.historial.StudentMajor.Student.Name + " " + n.historial.StudentMajor.Student.LastName,
                               userID = n.historial.StudentMajor.Student.UserId,
                               userPic = n.historial.StudentMajor.Student.ProfilePicturePath,
                               asignatura = n.historial.Asignatura.Asignatura.Codigo + "-" + n.historial.Asignatura.Asignatura.Name,
                               calif = n.historial.Calificacion

                           }).ToList();

                return data;
            }

        }
        public class changeDTO
        {
            public int newCalif { get; set; }
            public int historyLineId { get; set; }
            public int solicitudId { get; set; }
        }
        [HttpPut]
        public object changeCalif(changeDTO newCalif)
        {
            using (var context = new AcademicSystemContext())
            {
                var solicitud = context.SolicitudesRevisiones
                    .Where(s => s.SolRevisionID == newCalif.solicitudId)
                    .FirstOrDefault();

                var historial = context.StudentsHistories
                    .Where(h => h.HistoryLineId == newCalif.historyLineId)
                    .FirstOrDefault();



                context.Database.ExecuteSqlCommand(
                      " UPDATE [dbo].[solicitudRevisions]"
                    + " SET [calificacionAnterior] =" + historial.Calificacion
                    + ", [estado]= 1"
                    + " WHERE [SolRevisionID] = " + solicitud.SolRevisionID
                    );

                context.Database.ExecuteSqlCommand(
                     "UPDATE [dbo].[StudentHistories] "
                    + "SET [Calificacion]=" + newCalif.newCalif.ToString()
                    + " WHERE [HistoryLineId] =" + historial.HistoryLineId.ToString()
                    );
                //solicitud.calificacionAnterior = historial.Calificacion;
                //historial.Calificacion = newCalif.newCalif;
                //solicitud.estado = SchemaTypes.revisionStatus.Procede;

                //context.SaveChanges();
                return HttpStatusCode.OK;
            }
        }


        public class publishmentDTO
        {
            public int Nota { get; set; }
            public int studentID { get; set; }
            public int seccion { get; set; }
        }
        [HttpPut]
        public object publish(publishmentDTO @new)
        {
            using (var context = new AcademicSystemContext())
            {
                var studentMajor = context.StudentMajors.Where(m => m.Student.UserId == @new.studentID && m.Status == SchemaTypes.StudentMajorStatus.Cursando).First();

                context.Database.ExecuteSqlCommand(
                    " UPDATE [dbo].[StudentHistories] "
                    + " SET [Calificacion] =" + @new.Nota.ToString()
                    + " WHERE [Asignatura_PeriodAsignatureID] =" + @new.seccion
                    + " AND [StudentMajor_id] =" + studentMajor.id.ToString());

                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        // DELETE: api/SeccionesDePeriodos/5
        public void Delete(int id)
        {
        }
    }
}
