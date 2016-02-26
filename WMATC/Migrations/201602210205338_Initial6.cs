namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "HasBeenPairedDown", c => c.Boolean());
            AddColumn("dbo.Teams", "HasBeenBye", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "HasBeenBye");
            DropColumn("dbo.Teams", "HasBeenPairedDown");
        }
    }
}
