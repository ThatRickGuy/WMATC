namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "ListLockDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Matchups", "Player1CP", c => c.Int());
            AddColumn("dbo.Matchups", "Player2CP", c => c.Int());
            AddColumn("dbo.Matchups", "Player1APD", c => c.Int());
            AddColumn("dbo.Matchups", "Player2APD", c => c.Int());
            AddColumn("dbo.RoundTeamMatchups", "TableZone", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoundTeamMatchups", "TableZone");
            DropColumn("dbo.Matchups", "Player2APD");
            DropColumn("dbo.Matchups", "Player1APD");
            DropColumn("dbo.Matchups", "Player2CP");
            DropColumn("dbo.Matchups", "Player1CP");
            DropColumn("dbo.Events", "ListLockDate");
        }
    }
}
