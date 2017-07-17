namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validatinghorarios : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Horarios", "Asignatura_PeriodAsignatureID", "dbo.PeriodAsignatures");
            DropForeignKey("dbo.Horarios", "Aula_RoomID", "dbo.Rooms");
            DropIndex("dbo.Horarios", new[] { "Asignatura_PeriodAsignatureID" });
            DropIndex("dbo.Horarios", new[] { "Aula_RoomID" });
            AlterColumn("dbo.Horarios", "Asignatura_PeriodAsignatureID", c => c.Int(nullable: false));
            AlterColumn("dbo.Horarios", "Aula_RoomID", c => c.Int(nullable: false));
            CreateIndex("dbo.Horarios", "Asignatura_PeriodAsignatureID");
            CreateIndex("dbo.Horarios", "Aula_RoomID");
            AddForeignKey("dbo.Horarios", "Asignatura_PeriodAsignatureID", "dbo.PeriodAsignatures", "PeriodAsignatureID", cascadeDelete: true);
            AddForeignKey("dbo.Horarios", "Aula_RoomID", "dbo.Rooms", "RoomID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Horarios", "Aula_RoomID", "dbo.Rooms");
            DropForeignKey("dbo.Horarios", "Asignatura_PeriodAsignatureID", "dbo.PeriodAsignatures");
            DropIndex("dbo.Horarios", new[] { "Aula_RoomID" });
            DropIndex("dbo.Horarios", new[] { "Asignatura_PeriodAsignatureID" });
            AlterColumn("dbo.Horarios", "Aula_RoomID", c => c.Int());
            AlterColumn("dbo.Horarios", "Asignatura_PeriodAsignatureID", c => c.Int());
            CreateIndex("dbo.Horarios", "Aula_RoomID");
            CreateIndex("dbo.Horarios", "Asignatura_PeriodAsignatureID");
            AddForeignKey("dbo.Horarios", "Aula_RoomID", "dbo.Rooms", "RoomID");
            AddForeignKey("dbo.Horarios", "Asignatura_PeriodAsignatureID", "dbo.PeriodAsignatures", "PeriodAsignatureID");
        }
    }
}
