namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Addingperiodoactualalmenudeopciones : DbMigration
    {
        public override void Up()
        {
            Sql(
                  "INSERT INTO [dbo].[MenuOptions] "
                 + "([Title],[Link],[Icon],[Description],[allowedType],[order],[parent_id]) "
                 + "VALUES ('Período Actual','/ActualPeriodo','fa fa-calendar-o','Periodo en curso.','2','5',"
                 + "(SELECT id FROM [MenuOptions] WHERE Title = 'Periodos' ));");
        }

        public override void Down()
        {
        }
    }
}
