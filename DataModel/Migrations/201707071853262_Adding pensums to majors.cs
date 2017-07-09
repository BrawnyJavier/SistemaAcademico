namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingpensumstomajors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pensums", "Major_MajorID", c => c.Int());
            CreateIndex("dbo.Pensums", "Major_MajorID");
            AddForeignKey("dbo.Pensums", "Major_MajorID", "dbo.Majors", "MajorID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pensums", "Major_MajorID", "dbo.Majors");
            DropIndex("dbo.Pensums", new[] { "Major_MajorID" });
            DropColumn("dbo.Pensums", "Major_MajorID");
        }
    }
}
