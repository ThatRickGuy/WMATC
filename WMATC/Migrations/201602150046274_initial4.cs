namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Matchups", "Player1List", c => c.Int());
            AlterColumn("dbo.Matchups", "Player2List", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Matchups", "Player2List", c => c.Int(nullable: false));
            AlterColumn("dbo.Matchups", "Player1List", c => c.Int(nullable: false));
        }
    }
}
