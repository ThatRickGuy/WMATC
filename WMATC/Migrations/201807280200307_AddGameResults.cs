namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGameResults : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matchups", "GameLength", c => c.Int());
            AddColumn("dbo.Matchups", "FirstPlayerID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Matchups", "FirstPlayerID");
            DropColumn("dbo.Matchups", "GameLength");
        }
    }
}
