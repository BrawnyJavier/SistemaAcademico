namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixintitles : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE [dbo].[MenuOptions] SET [Title] = 'Administrar Aulas' WHERE [Title] = 'A�adir Aula';");
            Sql("UPDATE [dbo].[MenuOptions]  SET [Title] = 'Administrar Carreras' WHERE [Title] = 'A�adir Carrera';");
            Sql("UPDATE [dbo].[MenuOptions]  SET [Title] = 'Administrar �reas' WHERE [Title] = 'A�adir �rea';");
        }
        
        public override void Down()
        {
        }
    }
}
