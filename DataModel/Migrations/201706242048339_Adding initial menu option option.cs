namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addinginitialmenuoptionoption : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions( title, Link, icon, allowedType, [order], parent_id) VALUES('Añadir Opcion','/AddMenuOption','fa fa-plus','3','0','1')");
        }
        
        public override void Down()
        {
        }
    }
}
