namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Aulas : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions ([Title], [Link], [Icon], [allowedType], [order])" +
                "VALUES ('Aulas', '/Aulas', 'fa-university', '2', '3')");
        }
        
        public override void Down()
        {
        }
    }
}
