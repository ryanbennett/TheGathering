namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerLinkChange : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.VolunteerEvents", "LocationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VolunteerEvents", "LocationId", c => c.Int(nullable: false));
        }
    }
}
