namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class statushistories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentHistories", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentHistories", "Status");
        }
    }
}
