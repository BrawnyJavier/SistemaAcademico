namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adminasignaturesfix : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE [dbo].[MenuOptions] "
               +"SET [parent_id] = (SELECT id FROM MenuOptions WHERE Title = 'Asignaturas')"
               + "WHERE [id] = (SELECT id FROM MenuOptions WHERE Title = 'Administrar asignaturas')"



                );
        }
        
        public override void Down()
        {
        }
    }
}
