namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventMealSiteLinking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VolunteerEvents", "Location_Id", c => c.Int());
            CreateIndex("dbo.VolunteerEvents", "Location_Id");
            AddForeignKey("dbo.VolunteerEvents", "Location_Id", "dbo.MealSites", "Id");
            DropColumn("dbo.VolunteerEvents", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VolunteerEvents", "Location", c => c.String());
            DropForeignKey("dbo.VolunteerEvents", "Location_Id", "dbo.MealSites");
            DropIndex("dbo.VolunteerEvents", new[] { "Location_Id" });
            DropColumn("dbo.VolunteerEvents", "Location_Id");
        }
    }
}
