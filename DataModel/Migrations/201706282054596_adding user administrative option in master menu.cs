namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinguseradministrativeoptioninmastermenu : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[Description],[allowedType],[order],[parent_id])" +
               "VALUES ('Administrar usuarios','/AdministrarUsuarios','Administrative options in users whom have already been added to the system.','2','1',(SELECT dbo.MenuOptions.id FROM dbo.MenuOptions WHERE dbo.MenuOptions.Title = 'Usuarios'))");
        }
        
        public override void Down()
        {
        }
    }
}
