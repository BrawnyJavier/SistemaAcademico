namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingareasmenuoption : DbMigration
    {
        public override void Up()
        {
            Sql(
                "INSERT INTO [dbo].[MenuOptions] "
               + "([Title],[Link],[Icon],[Description],[allowedType],[order]) "
               + "VALUES('Áreas','NONE','fa fa-home','Areas del intec','2','4') "
               );
            Sql(
                "INSERT INTO [dbo].[MenuOptions] "
                + "([Title],[Link],[Description],[allowedType],[order],[parent_id]) "
                + "VALUES ('Añadir Área','/addArea','Adds Areas to the system','2','0',(SELECT id FROM dbo.MenuOptions WHERE Title = 'Áreas'))"
                );
        }
        
        public override void Down()
        {
        }
    }
}
