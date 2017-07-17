namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingareas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        AreaID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Codigo = c.String(),
                    })
                .PrimaryKey(t => t.AreaID);
            
            AddColumn("dbo.Majors", "FechaIntroduccion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Majors", "Area_AreaID", c => c.Int());
            CreateIndex("dbo.Majors", "Area_AreaID");
            AddForeignKey("dbo.Majors", "Area_AreaID", "dbo.Areas", "AreaID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Majors", "Area_AreaID", "dbo.Areas");
            DropIndex("dbo.Majors", new[] { "Area_AreaID" });
            DropColumn("dbo.Majors", "Area_AreaID");
            DropColumn("dbo.Majors", "FechaIntroduccion");
            DropTable("dbo.Areas");
        }
    }
}
