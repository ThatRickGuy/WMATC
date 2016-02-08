namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Matchups", "WinnerId", "dbo.Players");
            DropIndex("dbo.Matchups", new[] { "WinnerId" });
            AlterColumn("dbo.Matchups", "WinnerId", c => c.Int());
            CreateIndex("dbo.Matchups", "WinnerId");
            AddForeignKey("dbo.Matchups", "WinnerId", "dbo.Players", "PlayerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Matchups", "WinnerId", "dbo.Players");
            DropIndex("dbo.Matchups", new[] { "WinnerId" });
            AlterColumn("dbo.Matchups", "WinnerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Matchups", "WinnerId");
            AddForeignKey("dbo.Matchups", "WinnerId", "dbo.Players", "PlayerId", cascadeDelete: true);
        }
    }
}
