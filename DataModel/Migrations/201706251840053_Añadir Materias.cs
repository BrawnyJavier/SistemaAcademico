namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AñadirMaterias : DbMigration
    {
        public override void Up()
        {
            Sql("Insert Into dbo.MenuOptions ([Title], [Link], [Description], [allowedType], [order])" + 
                "VALUES ('Añadir Materia', '/AddAsignature', 'Add Asignature to the system', '2', '2')");

        }
        
        public override void Down()
        {

        }
    }
}
