namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingmenuoptionscascadeschemamod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuOptions", "order", c => c.Int(nullable: false));
            AddColumn("dbo.MenuOptions", "parent_id", c => c.Int());
            CreateIndex("dbo.MenuOptions", "parent_id");
            AddForeignKey("dbo.MenuOptions", "parent_id", "dbo.MenuOptions", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuOptions", "parent_id", "dbo.MenuOptions");
            DropIndex("dbo.MenuOptions", new[] { "parent_id" });
            DropColumn("dbo.MenuOptions", "parent_id");
            DropColumn("dbo.MenuOptions", "order");
        }
    }
}
