namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingrevisiones : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.solicitudRevisions",
                c => new
                    {
                        SolRevisionID = c.Int(nullable: false, identity: true),
                        fechaSolicitud = c.DateTime(nullable: false),
                        estado = c.Int(nullable: false),
                        fechaCancelacion = c.DateTime(nullable: true),
                        fechaReunion = c.DateTime(nullable: true),
                        calificacionAnterior = c.Int(nullable: false),
                        aceptadaEstudiante = c.Boolean(),
                        motivoSolicitud = c.String(),
                        AulaReunion_RoomID = c.Int(),
                        historial_HistoryLineId = c.Int(),
                    })
                .PrimaryKey(t => t.SolRevisionID)
                .ForeignKey("dbo.Rooms", t => t.AulaReunion_RoomID)
                .ForeignKey("dbo.StudentHistories", t => t.historial_HistoryLineId)
                .Index(t => t.AulaReunion_RoomID)
                .Index(t => t.historial_HistoryLineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.solicitudRevisions", "historial_HistoryLineId", "dbo.StudentHistories");
            DropForeignKey("dbo.solicitudRevisions", "AulaReunion_RoomID", "dbo.Rooms");
            DropIndex("dbo.solicitudRevisions", new[] { "historial_HistoryLineId" });
            DropIndex("dbo.solicitudRevisions", new[] { "AulaReunion_RoomID" });
            DropTable("dbo.solicitudRevisions");
        }
    }
}
