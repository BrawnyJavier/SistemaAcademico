namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingmenuoptionsforteachers : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[Icon],[allowedType],[order])" +
  "VALUES ('Publicación', '/Publicar','fa fa-balance-scale', '1', '0')");

            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[Icon],[allowedType],[order])" +
"VALUES ('Revision', '/Revisiones','fa fa-calendar', '1', '1')");

            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[Icon],[allowedType],[order])" +
"VALUES ('Preferencias', '/Publicar','fa fa-cogs', '1', '2')");
        }
        
        public override void Down()
        {
        }
    }
}
