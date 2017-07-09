namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingAddAsignaturaLogo : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.MenuOptions SET Icon ='fa fa-book' WHERE TITLE = 'Añadir Materia'");
        }
        
        public override void Down()
        {
        }
    }
}
