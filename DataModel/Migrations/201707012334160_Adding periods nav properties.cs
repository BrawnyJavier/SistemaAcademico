namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingperiodsnavproperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PeriodAsignatures", "Periodo_PeriodoID", c => c.Int());
            CreateIndex("dbo.PeriodAsignatures", "Periodo_PeriodoID");
            AddForeignKey("dbo.PeriodAsignatures", "Periodo_PeriodoID", "dbo.Periodoes", "PeriodoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PeriodAsignatures", "Periodo_PeriodoID", "dbo.Periodoes");
            DropIndex("dbo.PeriodAsignatures", new[] { "Periodo_PeriodoID" });
            DropColumn("dbo.PeriodAsignatures", "Periodo_PeriodoID");
        }
    }
}
