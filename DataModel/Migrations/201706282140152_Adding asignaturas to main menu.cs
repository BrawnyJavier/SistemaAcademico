namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Addingasignaturastomainmenu : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[Description],[allowedType],[order])" +
              "VALUES ('Asignaturas','/404','Administrates asignatues.','2','2')");
        }

        public override void Down()
        {
        }
    }
}
