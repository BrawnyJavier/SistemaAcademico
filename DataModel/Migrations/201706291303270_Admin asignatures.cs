namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adminasignatures : DbMigration
    {
        public override void Up()
        {
            Sql(
                "INSERT INTO [dbo].[MenuOptions] "
              + "(Title,Link,Description,[allowedType],[order]) " 
              + "VALUES('Administrar asignaturas','/adminAsignatures'," 
              + "'Administers asignatures in system','2','1')"
              );


        }
        
        public override void Down()
        {
        }
    }
}
