namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixintitles : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE [dbo].[MenuOptions] SET [Title] = 'Administrar Aulas' WHERE [Title] = 'Añadir Aula';");
            Sql("UPDATE [dbo].[MenuOptions]  SET [Title] = 'Administrar Carreras' WHERE [Title] = 'Añadir Carrera';");
            Sql("UPDATE [dbo].[MenuOptions]  SET [Title] = 'Administrar Áreas' WHERE [Title] = 'Añadir Área';");
        }
        
        public override void Down()
        {
        }
    }
}
