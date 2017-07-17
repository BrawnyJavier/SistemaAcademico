namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingtandastoteachers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "tandaProfesor", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "tandaProfesor");
        }
    }
}
