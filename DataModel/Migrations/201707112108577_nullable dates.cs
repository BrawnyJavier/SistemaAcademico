namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullabledates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.solicitudRevisions", "fechaCancelacion", c => c.DateTime());
            AlterColumn("dbo.solicitudRevisions", "fechaReunion", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.solicitudRevisions", "fechaReunion", c => c.DateTime(nullable: false));
            AlterColumn("dbo.solicitudRevisions", "fechaCancelacion", c => c.DateTime(nullable: false));
        }
    }
}
