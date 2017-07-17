namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingroomnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "RoomNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rooms", "RoomNumber");
        }
    }
}
