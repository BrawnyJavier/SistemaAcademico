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
             + "VALUES ('Añadir Periodo','/AddPeriodo','fa fa-calendar-o','Añadir un período','2','0',"
             + "(SELECT id FROM [MenuOptions] WHERE Title = 'Periodos' ));");
            Sql(
              "INSERT INTO [dbo].[MenuOptions] "
             + "([Title],[Link],[Icon],[Description],[allowedType],[order],[parent_id]) "
             + "VALUES ('Administración de Períodos','/AdminPeriodo','fa fa-calendar-o','Añadir un periodo','2','0',"
             + "(SELECT id FROM [MenuOptions] WHERE Title = 'Periodos' ));");
        }
        
        public override void Down()
        {
        }
    }
}
