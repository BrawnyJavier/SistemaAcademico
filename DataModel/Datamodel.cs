using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaAcademico.Classes;

namespace SistemaAcademico.DataModel
{
    public class AcademicSystemContext : DbContext
    {
        public DbSet<User> Usuarios { get; set; }
        public DbSet<MenuOption> OpcionesDelMenu { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Honor> Honors { get; set; }
        public DbSet<StudentMajor> StudentMajors { get; set; }
        public DbSet<PeriodAsignature> PeriodAsignature { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Preseleccion> Preselecciones { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Requisito> Requisitos { get; set; }
        public DbSet<Trimestre> Trimestres { get; set; }
        public DbSet<StudentHistory> StudentsHistories { get; set; }
        public DbSet<Pensum> Pensums { get; set; }
        public DbSet<AsignatureTeacher> AsignatureTeachers { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<solicitudRevision> SolicitudesRevisiones { get; set; }

    }
}
