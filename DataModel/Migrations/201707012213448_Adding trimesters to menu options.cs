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
                + "VALUES ('Periodos','NONE','fa fa-calendar-o','Administración de períodos','2','5');"
                );
        }
        
        public override void Down()
        {
        }
    }
}
