namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addinginitialmenuoptionoption2Refactor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "BirthDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "PhoneNum", c => c.Long(nullable: false));
            DropColumn("dbo.Users", "BirhtdaDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "BirhtdaDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "PhoneNum", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "BirthDate");
        }
    }
}
