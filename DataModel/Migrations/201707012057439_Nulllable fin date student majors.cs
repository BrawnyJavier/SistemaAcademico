namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nulllablefindatestudentmajors : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StudentMajors", "FinDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StudentMajors", "FinDate", c => c.DateTime(nullable: false));
        }
    }
}
