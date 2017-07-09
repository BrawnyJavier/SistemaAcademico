namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingtrimesterstomenuoptions : DbMigration
    {
        public override void Up()
        {
            Sql(
                 "INSERT INTO [dbo].[MenuOptions] "
                +"([Title],[Link],[Icon],[Description],[allowedType],[order]) "
                + "VALUES ('Periodos','NONE','fa fa-calendar-o','Administraci�n de per�odos','2','5');"
                );
        }
        
        public override void Down()
        {
        }
    }
}
