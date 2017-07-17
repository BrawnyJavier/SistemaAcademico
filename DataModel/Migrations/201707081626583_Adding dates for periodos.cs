namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingdatesforperiodos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Periodoes", "fechaInicioPreselecion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Periodoes", "fechafinPreseleccion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Periodoes", "fechainicioSeleccion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Periodoes", "fechaLimiteRetiro", c => c.DateTime(nullable: false));
            AddColumn("dbo.Periodoes", "fechafinSeleccion", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Periodoes", "fechafinSeleccion");
            DropColumn("dbo.Periodoes", "fechaLimiteRetiro");
            DropColumn("dbo.Periodoes", "fechainicioSeleccion");
            DropColumn("dbo.Periodoes", "fechafinPreseleccion");
            DropColumn("dbo.Periodoes", "fechaInicioPreselecion");
        }
    }
}
