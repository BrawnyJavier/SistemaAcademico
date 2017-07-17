namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tandasasignaturas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PeriodAsignatures", "Tanda", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PeriodAsignatures", "Tanda");
        }
    }
}
