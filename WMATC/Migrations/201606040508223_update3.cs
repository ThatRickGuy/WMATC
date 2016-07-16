namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "EventDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Events", "ListLockDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "ListLockDate", c => c.DateTime());
            AlterColumn("dbo.Events", "EventDate", c => c.DateTime());
        }
    }
}
