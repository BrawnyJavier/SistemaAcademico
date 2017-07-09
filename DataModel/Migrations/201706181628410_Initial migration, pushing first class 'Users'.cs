namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialmigrationpushingfirstclassUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Name2 = c.String(),
                        LastName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 450),
                        BirhtdaDate = c.DateTime(nullable: false),
                        PhoneNum = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        UserType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Email, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            DropTable("dbo.Users");
        }
    }
}
