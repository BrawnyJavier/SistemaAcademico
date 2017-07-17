namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingPeriodstoschema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Periodoes",
                c => new
                    {
                        PeriodoID = c.Int(nullable: false, identity: true),
                        fechaInicio = c.DateTime(nullable: false),
                        fechaFin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PeriodoID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Periodoes");
        }
    }
}
