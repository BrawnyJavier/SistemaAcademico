namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class fordannyfix : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions ([Title], [Link], [Icon], [allowedType], [order])" +
             "VALUES ('Aulas', '/Aulas', 'fa fa-university', '2', '3')");
            Sql(
                "UPDATE dbo.MenuOptions " +
                "SET dbo.MenuOptions.[Link] = '/AddAula',  " +
                "dbo.MenuOptions.[parent_id] = (Select Id FROM dbo.MenuOptions WHERE TITLE = 'Aulas')" +
                "WHERE dbo.MenuOptions.[Title] = 'Añadir Aula' "
                );



        }

        public override void Down()
        {
        }
    }
}
