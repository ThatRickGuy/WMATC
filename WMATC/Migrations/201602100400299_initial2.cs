namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "FactionId", c => c.Int());
            AddColumn("dbo.Players", "Caster1", c => c.String());
            AddColumn("dbo.Players", "Caster2", c => c.String());
            CreateIndex("dbo.Players", "FactionId");
            AddForeignKey("dbo.Players", "FactionId", "dbo.Factions", "FactionId");
            DropColumn("dbo.Players", "ImgURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "ImgURL", c => c.String());
            DropForeignKey("dbo.Players", "FactionId", "dbo.Factions");
            DropIndex("dbo.Players", new[] { "FactionId" });
            DropColumn("dbo.Players", "Caster2");
            DropColumn("dbo.Players", "Caster1");
            DropColumn("dbo.Players", "FactionId");
        }
    }
}
