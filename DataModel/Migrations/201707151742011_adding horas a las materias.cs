namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinghorasalasmaterias : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Asignaturas", "HorasSemanales", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Asignaturas", "HorasSemanales");
        }
    }
}
