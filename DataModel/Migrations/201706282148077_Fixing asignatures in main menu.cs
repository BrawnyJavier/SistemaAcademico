namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixingasignaturesinmainmenu : DbMigration
    {
        public override void Up()
        {
            Sql(
                "UPDATE [dbo].[MenuOptions]"+
                "SET [Icon] = 'fa fa-book' "+
                "WHERE [dbo].[MenuOptions].id = 9;" +
                
                "UPDATE [dbo].[MenuOptions]" +
                "SET [parent_id] = 9,"+
                "[Title] = 'A�adir Asignatura'"+
                "WHERE [dbo].[MenuOptions].id = 7;"
                );
        }
        
        public override void Down()
        {
        }
    }
}
