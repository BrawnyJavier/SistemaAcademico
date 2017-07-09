namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingUsersMenuOption : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions"+
                "(Title, Link, Icon, allowedType, [order])"+
                "VALUES('Usuarios','NONE','fa fa-users','3','1')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM dbo.MenuOptions"+
                "WHERE Title = 'Usuarios'");
        }
    }
}
