namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingrevisioncalif : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[allowedType],[order], [parent_id])" +
           "VALUES ('Revisión de calificaciones', '/Revision', '0', '10', (SELECT id FROM dbo.MenuOptions WHERE Title = 'Procesos Académicos'))");
        }
        
        public override void Down()
        {
        }
    }
}
