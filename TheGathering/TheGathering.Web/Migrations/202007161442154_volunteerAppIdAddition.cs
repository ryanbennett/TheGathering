namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class volunteerAppIdAddition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Volunteers", "ApplicationUserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Volunteers", "ApplicationUserId");
        }
    }
}
