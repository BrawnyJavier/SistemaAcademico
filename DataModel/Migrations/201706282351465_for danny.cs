namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fordanny : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions ([Title], [Link], [Description], [allowedType], [order], [parent_id])" +
                "VALUES ('A�adir Aula', '/A�adir Aula','A�ade un aula al sistema', '2', '1',(Select Id FROM dbo.MenuOptions WHERE TITLE = 'Aulas'))");
        }
        
        public override void Down()
        {
        }
    }
}
