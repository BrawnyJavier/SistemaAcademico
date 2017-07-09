namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Añadiraulayupdateicon : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions ([Title], [Link], [Description], [allowedType], [order], [parent_id])" +
                "VALUES ('Añadir Aula', '/Añadir Aula','Añade un aula al sistema', '2', '1','9')");

            Sql("Update dbo.MenuOptions Set Icon = 'fa fa-university' WHERE TITLE = 'Aulas' ");
        }
        
        public override void Down()
        {
        }
    }
}
