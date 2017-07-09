namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingmenuoptionsusertypevalidation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuOptions", "allowedType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuOptions", "allowedType");
        }
    }
}
