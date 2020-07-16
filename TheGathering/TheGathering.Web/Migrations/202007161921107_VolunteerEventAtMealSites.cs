namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerEventAtMealSites : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VolunteerEvents", "MealSite_Id", "dbo.MealSites");
            DropIndex("dbo.VolunteerEvents", new[] { "MealSite_Id" });
            DropColumn("dbo.VolunteerEvents", "MealSite_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VolunteerEvents", "MealSite_Id", c => c.Int());
            CreateIndex("dbo.VolunteerEvents", "MealSite_Id");
            AddForeignKey("dbo.VolunteerEvents", "MealSite_Id", "dbo.MealSites", "Id");
        }
    }
}
