namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class themes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Theme1", c => c.String());
            AddColumn("dbo.Players", "Theme2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "Theme2");
            DropColumn("dbo.Players", "Theme1");
        }
    }
}
