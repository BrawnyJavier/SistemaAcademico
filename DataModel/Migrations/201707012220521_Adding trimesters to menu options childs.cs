namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingtrimesterstomenuoptionschilds : DbMigration
    {
        public override void Up()
        {
            Sql(
              "INSERT INTO [dbo].[MenuOptions] "
             + "([Title],[Link],[Icon],[Description],[allowedType],[order],[parent_id]) "
             + "VALUES ('A�adir Periodo','/AddPeriodo','fa fa-calendar-o','A�adir un per�odo','2','0',"
             + "(SELECT id FROM [MenuOptions] WHERE Title = 'Periodos' ));");
            Sql(
              "INSERT INTO [dbo].[MenuOptions] "
             + "([Title],[Link],[Icon],[Description],[allowedType],[order],[parent_id]) "
             + "VALUES ('Administraci�n de Per�odos','/AdminPeriodo','fa fa-calendar-o','A�adir un periodo','2','0',"
             + "(SELECT id FROM [MenuOptions] WHERE Title = 'Periodos' ));");
        }
        
        public override void Down()
        {
        }
    }
}
