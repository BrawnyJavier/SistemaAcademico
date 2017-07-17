namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingstatustoperiods : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Periodoes", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Periodoes", "Status");
        }
    }
}
