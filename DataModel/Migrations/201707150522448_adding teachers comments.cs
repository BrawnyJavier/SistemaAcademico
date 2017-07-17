namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingteacherscomments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.solicitudRevisions", "ComentarioProfesor", c => c.String());
            AddColumn("dbo.solicitudRevisions", "ComentarioAdmin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.solicitudRevisions", "ComentarioAdmin");
            DropColumn("dbo.solicitudRevisions", "ComentarioProfesor");
        }
    }
}
