namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingcodigo2asignatures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Asignaturas", "Codigo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Asignaturas", "Codigo");
        }
    }
}
