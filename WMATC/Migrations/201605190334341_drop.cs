namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "DropRound", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "DropRound");
        }
    }
}
