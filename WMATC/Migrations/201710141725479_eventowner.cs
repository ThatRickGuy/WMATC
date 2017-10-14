namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eventowner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Owner", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Owner");
        }
    }
}
