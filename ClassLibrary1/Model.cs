using SistemaAcademico.SchemaTypes;
using SistemaAcademico.FixedValues;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using SistemaAcademico.Classes;

namespace SistemaAcademico.Classes

{
    #region UsersModule
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Name2 { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [EmailAddress]
        [StringLength(450)]
        [Index(IsUnique = true)]
        [Index]
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public long PhoneNum { get; set; }
        [Required]
        public sex Sex { get; set; }
        [Required]
        public UserTypes UserType { get; set; }
        public string ProfilePicturePath { get; set; }
    }
    public class Area
    {
        [Key]
        public int AreaID { get; set; }
        public string Name { get; set; }
        public List<Major> Carreras { get; set; }
        public string Codigo { get; set; }
    }
    public class Major
    {
        [Key]
        public int MajorID { get; set; }
        public string MajorTitle { get; set; }
        public string Description { get; set; }
        public int CreditsCount { get; set; }
        public Area Area { get; set; }
        public DateTime FechaIntroduccion { get; set; }
        public List<Pensum> Pensum { get; set; }
    }
    public class Honor
    {
        [Key]
        public int HonorID { get; set; }
        public string Name { get; set; }
        public float MinGpa { get; set; }
        public float MaxGPA { get; set; }
    }
    public class StudentMajor
    {
        public int id { get; set; }
        public User Student { get; set; }
        public Major Major { get; set; }
        public float GPA { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime? FinDate { get; set; }
        public StudentMajorStatus Status { get; set; }
        public Honor Honor { get; set; }
    }
    public class PeriodAsignature
    {
        public int PeriodAsignatureID { get; set; }
        public Trimestre Trimestre { get; set; }
        public Asignatura Asignatura { get; set; }
        public User Profesor { get; set; }
        public Periodo Periodo { get; set; }
        public int? StudentsCount { get; set; }
        public int seccion { get; set; }
        public IEnumerable<Horario> Horarios { get; set; }

    }
    public class Periodo
    {
        [Key]
        public int PeriodoID { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public List<PeriodAsignature> periodAsignatures { get; set; }
        public PeriodStatus Status { get; set; }
        public DateTime fechaInicioPreselecion { get; set; }
        public DateTime fechafinPreseleccion { get; set; }
        public DateTime fechainicioSeleccion { get; set; }
        public DateTime fechaLimiteRetiro { get; set; }
        public DateTime fechafinSeleccion { get; set; }
    }
    public class Horario
    {
        [Key]
        public int HorarioID { get; set; }
        [Required]
        public int Day { get; set; }
        [Required]
        public DateTime HourInit { get; set; }
        [Required]
        public DateTime HourUntil { get; set; }
        [Required]
        public PeriodAsignature Asignatura { get; set; }
        [Required]
        public Room Aula { get; set; }
    }
    public class Preseleccion
    {
        [Key]
        public int PreseleccionID { get; set; }
        public PeriodAsignature Asignatura { get; set; }
        public User Student { get; set; }
        public Tanda TandaDeseada { get; set; }
    }
    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        public int Asientos { get; set; }
        public string Building { get; set; }
        public int RoomNumber { get; set; }
    }
    public class Asignatura
    {
        [Key]
        public int AsignatureID { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public AsignatureTypes TipoAsignatura { get; set; }
        public string Codigo { get; set; }

    }
    public class solicitudRevision
    {
        [Key]
        public int SolRevisionID { get; set; }
        public StudentHistory historial { get; set; }
        public DateTime fechaSolicitud { get; set; }
        public revisionStatus estado { get; set; }
        public DateTime fechaCancelacion { get; set; }
        public DateTime fechaReunion { get; set; }
        public Room AulaReunion { get; set; }
        public int calificacionAnterior { get; set; }
        public bool? aceptadaEstudiante { get; set; }
        public string motivoSolicitud { get; set; }
    }
    public class Requisito
    {
        [Key]
        public int RequisitoID { get; set; }
        public Asignatura Asignatura { get; set; }
        public RequisitoTypes TipoRequisito { get; set; }
        public int ReqCreditos { get; set; }
        public Asignatura ReqAsignatura { get; set; }
    }
    public class Trimestre
    {
        public int TrimestreID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
    public class StudentHistory
    {
        [Key]
        public int HistoryLineId { get; set; }
        [Required]
        public StudentMajor StudentMajor { get; set; }
        [Required]
        public PeriodAsignature Asignatura { get; set; }
        public int Calificacion { get; set; }
        public string CalificacionAlpha { get; set; }
        public HistorialStatus Status { get; set; }
    }
    public class Pensum
    {
        [Key]
        public int PensumID { get; set; }
        public Asignatura Asignatura { get; set; }
        public int TrimestreOrder { get; set; }
        public Major Major { get; set; }
    }
    public class AsignatureTeacher
    {
        public int id { get; set; }
        public User Teacher { get; set; }
        public Asignatura Asignatura { get; set; }
        public DateTime AsignDate { get; set; }
    }
    #endregion
    #region sysmodule
    public class MenuOption
    {
        public int id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Link { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public UserTypes allowedType { get; set; }
        public MenuOption parent { get; set; }
        public int order { get; set; }
    }
    #endregion
}
