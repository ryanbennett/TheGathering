namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryingtheIntx2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VolunteerEvents", "LocationId", c => c.Int(nullable: false));
            DropColumn("dbo.VolunteerEvents", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VolunteerEvents", "Location", c => c.Int(nullable: false));
            DropColumn("dbo.VolunteerEvents", "LocationId");
        }
    }
}
