namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addingmajorstomenu : DbMigration
    {
        public override void Up()
        {
            Sql(
                 "INSERT INTO [dbo].[MenuOptions] "
                +"([Title],[Link],[Icon],[Description],[allowedType],[order]) "
                + "VALUES('Carreras','NONE','fa fa-graduation-cap','Carreras impartidas en el intec','2','3') "
                );
            Sql(
                "INSERT INTO [dbo].[MenuOptions] "
                + "([Title],[Link],[Description],[allowedType],[order],[parent_id]) "
                + "VALUES ('Añadir carrera','/addMajor','Adds majors to the system','2','0',(SELECT id FROM dbo.MenuOptions WHERE Title = 'Carreras'))"
                );
        }

        public override void Down()
        {
        }
    }
}
