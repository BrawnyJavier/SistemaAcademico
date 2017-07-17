namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class droppingmenuoptions : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM [dbo].[MenuOptions] " +
                "WHERE [parent_id] = (SELECT id FROM dbo.MenuOptions WHERE Title = 'Opciones de Menu')");

            Sql("DELETE FROM [dbo].[MenuOptions] "+
                "WHERE [Title] = 'Opciones de Menu'");
        }
        
        public override void Down()
        {
        }
    }
}
