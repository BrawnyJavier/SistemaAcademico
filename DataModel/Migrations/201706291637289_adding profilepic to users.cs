namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingprofilepictousers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ProfilePicturePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ProfilePicturePath");
        }
    }
}
