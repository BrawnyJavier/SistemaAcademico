namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingaulastohorarios : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PeriodAsignatures", "Aula_RoomID", "dbo.Rooms");
            DropIndex("dbo.PeriodAsignatures", new[] { "Aula_RoomID" });
            AddColumn("dbo.Horarios", "Aula_RoomID", c => c.Int());
            AlterColumn("dbo.PeriodAsignatures", "StudentsCount", c => c.Int());
            CreateIndex("dbo.Horarios", "Aula_RoomID");
            AddForeignKey("dbo.Horarios", "Aula_RoomID", "dbo.Rooms", "RoomID");
            DropColumn("dbo.PeriodAsignatures", "Aula_RoomID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PeriodAsignatures", "Aula_RoomID", c => c.Int());
            DropForeignKey("dbo.Horarios", "Aula_RoomID", "dbo.Rooms");
            DropIndex("dbo.Horarios", new[] { "Aula_RoomID" });
            AlterColumn("dbo.PeriodAsignatures", "StudentsCount", c => c.Int(nullable: false));
            DropColumn("dbo.Horarios", "Aula_RoomID");
            CreateIndex("dbo.PeriodAsignatures", "Aula_RoomID");
            AddForeignKey("dbo.PeriodAsignatures", "Aula_RoomID", "dbo.Rooms", "RoomID");
        }
    }
}
