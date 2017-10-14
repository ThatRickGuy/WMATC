namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JSONDump1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "JSONDump", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "JSONDump");
        }
    }
}
