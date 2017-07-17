namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingestudentsoptionmenu : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[Icon],[allowedType],[order])" +
    "VALUES ('Procesos Académicos', 'NONE','fa fa-cogs', '0', '0')");
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[allowedType],[order], [parent_id])" +
                "VALUES ('Preselección', '/Preseleccion', '0', '0', (SELECT id FROM dbo.MenuOptions WHERE Title = 'Procesos Académicos'))");
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[allowedType],[order], [parent_id])" +
                "VALUES ('Selección', '/Seleccion', '0', '1', (SELECT id FROM dbo.MenuOptions WHERE Title = 'Procesos Académicos'))");
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[allowedType],[order], [parent_id])" +
                "VALUES ('Retiro', '/Retiro', '0', '2', (SELECT id FROM dbo.MenuOptions WHERE Title = 'Procesos Académicos'))");
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[Icon],[allowedType],[order])" +
                "VAlUES('Oferta Academica', 'NONE', 'fa fa-list-alt', '0', '1')");
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[Icon],[allowedType],[order])" +
                "VALUES('Programa de Asignaturas', 'NONE', 'fa fa-th-list', '0', '2')");
        }
        
        public override void Down()
        {
        }
    }
}
