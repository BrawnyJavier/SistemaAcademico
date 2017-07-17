namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DAsjdfuoqsfcos : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.MenuOptions ([Title],[Link],[Icon],[allowedType],[order])" +
"VALUES ('Historial Académico', '/Hisorial','fa fa-history', '0', '10')");
        }
        
        public override void Down()
        {
        }
    }
}
