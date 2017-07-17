namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CopyPastingSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Asignaturas",
                c => new
                    {
                        AsignatureID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Credits = c.Int(nullable: false),
                        TipoAsignatura = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AsignatureID);
            
            CreateTable(
                "dbo.AsignatureTeachers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        AsignDate = c.DateTime(nullable: false),
                        Asignatura_AsignatureID = c.Int(),
                        Teacher_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Asignaturas", t => t.Asignatura_AsignatureID)
                .ForeignKey("dbo.Users", t => t.Teacher_UserId)
                .Index(t => t.Asignatura_AsignatureID)
                .Index(t => t.Teacher_UserId);
            
            CreateTable(
                "dbo.Honors",
                c => new
                    {
                        HonorID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MinGpa = c.Single(nullable: false),
                        MaxGPA = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.HonorID);
            
            CreateTable(
                "dbo.Horarios",
                c => new
                    {
                        HorarioID = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        HourInit = c.DateTime(nullable: false),
                        HourUntil = c.DateTime(nullable: false),
                        Asignatura_PeriodAsignatureID = c.Int(),
                    })
                .PrimaryKey(t => t.HorarioID)
                .ForeignKey("dbo.PeriodAsignatures", t => t.Asignatura_PeriodAsignatureID)
                .Index(t => t.Asignatura_PeriodAsignatureID);
            
            CreateTable(
                "dbo.PeriodAsignatures",
                c => new
                    {
                        PeriodAsignatureID = c.Int(nullable: false, identity: true),
                        StudentsCount = c.Int(nullable: false),
                        seccion = c.Int(nullable: false),
                        Asignatura_AsignatureID = c.Int(),
                        Aula_RoomID = c.Int(),
                        Profesor_UserId = c.Int(),
                        Trimestre_TrimestreID = c.Int(),
                    })
                .PrimaryKey(t => t.PeriodAsignatureID)
                .ForeignKey("dbo.Asignaturas", t => t.Asignatura_AsignatureID)
                .ForeignKey("dbo.Rooms", t => t.Aula_RoomID)
                .ForeignKey("dbo.Users", t => t.Profesor_UserId)
                .ForeignKey("dbo.Trimestres", t => t.Trimestre_TrimestreID)
                .Index(t => t.Asignatura_AsignatureID)
                .Index(t => t.Aula_RoomID)
                .Index(t => t.Profesor_UserId)
                .Index(t => t.Trimestre_TrimestreID);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomID = c.Int(nullable: false, identity: true),
                        Asientos = c.Int(nullable: false),
                        Building = c.String(),
                    })
                .PrimaryKey(t => t.RoomID);
            
            CreateTable(
                "dbo.Trimestres",
                c => new
                    {
                        TrimestreID = c.Int(nullable: false, identity: true),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TrimestreID);
            
            CreateTable(
                "dbo.Majors",
                c => new
                    {
                        MajorID = c.Int(nullable: false, identity: true),
                        MajorTitle = c.String(),
                        Description = c.String(),
                        CreditsCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MajorID);
            
            CreateTable(
                "dbo.Pensums",
                c => new
                    {
                        PensumID = c.Int(nullable: false, identity: true),
                        TrimestreOrder = c.Int(nullable: false),
                        Asignatura_AsignatureID = c.Int(),
                    })
                .PrimaryKey(t => t.PensumID)
                .ForeignKey("dbo.Asignaturas", t => t.Asignatura_AsignatureID)
                .Index(t => t.Asignatura_AsignatureID);
            
            CreateTable(
                "dbo.Preseleccions",
                c => new
                    {
                        PreseleccionID = c.Int(nullable: false, identity: true),
                        TandaDeseada = c.Int(nullable: false),
                        Asignatura_PeriodAsignatureID = c.Int(),
                        Student_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.PreseleccionID)
                .ForeignKey("dbo.PeriodAsignatures", t => t.Asignatura_PeriodAsignatureID)
                .ForeignKey("dbo.Users", t => t.Student_UserId)
                .Index(t => t.Asignatura_PeriodAsignatureID)
                .Index(t => t.Student_UserId);
            
            CreateTable(
                "dbo.Requisitoes",
                c => new
                    {
                        RequisitoID = c.Int(nullable: false, identity: true),
                        TipoRequisito = c.Int(nullable: false),
                        ReqCreditos = c.Int(nullable: false),
                        Asignatura_AsignatureID = c.Int(),
                        ReqAsignatura_AsignatureID = c.Int(),
                    })
                .PrimaryKey(t => t.RequisitoID)
                .ForeignKey("dbo.Asignaturas", t => t.Asignatura_AsignatureID)
                .ForeignKey("dbo.Asignaturas", t => t.ReqAsignatura_AsignatureID)
                .Index(t => t.Asignatura_AsignatureID)
                .Index(t => t.ReqAsignatura_AsignatureID);
            
            CreateTable(
                "dbo.StudentMajors",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        GPA = c.Single(nullable: false),
                        InitDate = c.DateTime(nullable: false),
                        FinDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Honor_HonorID = c.Int(),
                        Major_MajorID = c.Int(),
                        Student_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Honors", t => t.Honor_HonorID)
                .ForeignKey("dbo.Majors", t => t.Major_MajorID)
                .ForeignKey("dbo.Users", t => t.Student_UserId)
                .Index(t => t.Honor_HonorID)
                .Index(t => t.Major_MajorID)
                .Index(t => t.Student_UserId);
            
            CreateTable(
                "dbo.StudentHistories",
                c => new
                    {
                        HistoryLineId = c.Int(nullable: false, identity: true),
                        Calificacion = c.Int(nullable: false),
                        CalificacionAlpha = c.String(),
                        Asignatura_PeriodAsignatureID = c.Int(nullable: false),
                        StudentMajor_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HistoryLineId)
                .ForeignKey("dbo.PeriodAsignatures", t => t.Asignatura_PeriodAsignatureID, cascadeDelete: true)
                .ForeignKey("dbo.StudentMajors", t => t.StudentMajor_id, cascadeDelete: true)
                .Index(t => t.Asignatura_PeriodAsignatureID)
                .Index(t => t.StudentMajor_id);
            
            AlterColumn("dbo.MenuOptions", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.MenuOptions", "Link", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentHistories", "StudentMajor_id", "dbo.StudentMajors");
            DropForeignKey("dbo.StudentHistories", "Asignatura_PeriodAsignatureID", "dbo.PeriodAsignatures");
            DropForeignKey("dbo.StudentMajors", "Student_UserId", "dbo.Users");
            DropForeignKey("dbo.StudentMajors", "Major_MajorID", "dbo.Majors");
            DropForeignKey("dbo.StudentMajors", "Honor_HonorID", "dbo.Honors");
            DropForeignKey("dbo.Requisitoes", "ReqAsignatura_AsignatureID", "dbo.Asignaturas");
            DropForeignKey("dbo.Requisitoes", "Asignatura_AsignatureID", "dbo.Asignaturas");
            DropForeignKey("dbo.Preseleccions", "Student_UserId", "dbo.Users");
            DropForeignKey("dbo.Preseleccions", "Asignatura_PeriodAsignatureID", "dbo.PeriodAsignatures");
            DropForeignKey("dbo.Pensums", "Asignatura_AsignatureID", "dbo.Asignaturas");
            DropForeignKey("dbo.Horarios", "Asignatura_PeriodAsignatureID", "dbo.PeriodAsignatures");
            DropForeignKey("dbo.PeriodAsignatures", "Trimestre_TrimestreID", "dbo.Trimestres");
            DropForeignKey("dbo.PeriodAsignatures", "Profesor_UserId", "dbo.Users");
            DropForeignKey("dbo.PeriodAsignatures", "Aula_RoomID", "dbo.Rooms");
            DropForeignKey("dbo.PeriodAsignatures", "Asignatura_AsignatureID", "dbo.Asignaturas");
            DropForeignKey("dbo.AsignatureTeachers", "Teacher_UserId", "dbo.Users");
            DropForeignKey("dbo.AsignatureTeachers", "Asignatura_AsignatureID", "dbo.Asignaturas");
            DropIndex("dbo.StudentHistories", new[] { "StudentMajor_id" });
            DropIndex("dbo.StudentHistories", new[] { "Asignatura_PeriodAsignatureID" });
            DropIndex("dbo.StudentMajors", new[] { "Student_UserId" });
            DropIndex("dbo.StudentMajors", new[] { "Major_MajorID" });
            DropIndex("dbo.StudentMajors", new[] { "Honor_HonorID" });
            DropIndex("dbo.Requisitoes", new[] { "ReqAsignatura_AsignatureID" });
            DropIndex("dbo.Requisitoes", new[] { "Asignatura_AsignatureID" });
            DropIndex("dbo.Preseleccions", new[] { "Student_UserId" });
            DropIndex("dbo.Preseleccions", new[] { "Asignatura_PeriodAsignatureID" });
            DropIndex("dbo.Pensums", new[] { "Asignatura_AsignatureID" });
            DropIndex("dbo.PeriodAsignatures", new[] { "Trimestre_TrimestreID" });
            DropIndex("dbo.PeriodAsignatures", new[] { "Profesor_UserId" });
            DropIndex("dbo.PeriodAsignatures", new[] { "Aula_RoomID" });
            DropIndex("dbo.PeriodAsignatures", new[] { "Asignatura_AsignatureID" });
            DropIndex("dbo.Horarios", new[] { "Asignatura_PeriodAsignatureID" });
            DropIndex("dbo.AsignatureTeachers", new[] { "Teacher_UserId" });
            DropIndex("dbo.AsignatureTeachers", new[] { "Asignatura_AsignatureID" });
            AlterColumn("dbo.MenuOptions", "Link", c => c.String());
            AlterColumn("dbo.MenuOptions", "Title", c => c.String());
            DropTable("dbo.StudentHistories");
            DropTable("dbo.StudentMajors");
            DropTable("dbo.Requisitoes");
            DropTable("dbo.Preseleccions");
            DropTable("dbo.Pensums");
            DropTable("dbo.Majors");
            DropTable("dbo.Trimestres");
            DropTable("dbo.Rooms");
            DropTable("dbo.PeriodAsignatures");
            DropTable("dbo.Horarios");
            DropTable("dbo.Honors");
            DropTable("dbo.AsignatureTeachers");
            DropTable("dbo.Asignaturas");
        }
    }
}
