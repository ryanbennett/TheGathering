namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerEventMealSite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VolunteerEvents", "LocationId", c => c.Int(nullable: false));
            AddColumn("dbo.VolunteerEvents", "MealSite_Id", c => c.Int());
            CreateIndex("dbo.VolunteerEvents", "MealSite_Id");
            AddForeignKey("dbo.VolunteerEvents", "MealSite_Id", "dbo.MealSites", "Id");
            DropColumn("dbo.VolunteerEvents", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VolunteerEvents", "Location", c => c.String());
            DropForeignKey("dbo.VolunteerEvents", "MealSite_Id", "dbo.MealSites");
            DropIndex("dbo.VolunteerEvents", new[] { "MealSite_Id" });
            DropColumn("dbo.VolunteerEvents", "MealSite_Id");
            DropColumn("dbo.VolunteerEvents", "LocationId");
        }
    }
}
