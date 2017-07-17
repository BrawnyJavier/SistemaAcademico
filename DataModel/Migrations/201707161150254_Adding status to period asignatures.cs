namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingstatustoperiodasignatures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PeriodAsignatures", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PeriodAsignatures", "Status");
        }
    }
}
