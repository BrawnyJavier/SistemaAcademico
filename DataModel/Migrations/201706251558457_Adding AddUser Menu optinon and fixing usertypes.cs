namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddingAddUserMenuoptinonandfixingusertypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[Description],[allowedType],[order],[parent_id])" +
                "VALUES ('Añadir usuarios','/AddUSer','Adds users to system','2','0',(SELECT dbo.MenuOptions.id FROM dbo.MenuOptions WHERE dbo.MenuOptions.Title = 'Usuarios'))");
            Sql("UPDATE dbo.Menuoptions SET [allowedType] =2");
        }

        public override void Down()
        {
        }
    }
}
