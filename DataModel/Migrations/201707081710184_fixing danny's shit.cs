namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingdannysshit : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE [dbo].[MenuOptions] SET [Link] = '/Programas' WHERE [Title] = 'Programa de Asignaturas';");
            Sql("UPDATE [dbo].[MenuOptions] SET [Link] = '/OfertaAcademica', 	[Title] = 'Oferta Académica' WHERE [Title] = 'Oferta Academica'; ");
        }
        
        public override void Down()
        {
        }
    }
}
