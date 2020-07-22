namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeperateMeals : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MealSites", "Name", c => c.String());
            AddColumn("dbo.MealSites", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.MealSites", "Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.MealSites", "IsTheGatheringSite", c => c.Boolean(nullable: false));
            AddColumn("dbo.MealSites", "Breakfast_DaysServed", c => c.String());
            AddColumn("dbo.MealSites", "Breakfast_MaximumGuestsServed", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "Breakfast_MinimumGuestsServed", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "Breakfast_StartTime", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "Breakfast_EndTime", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "Lunch_DaysServed", c => c.String());
            AddColumn("dbo.MealSites", "Lunch_MaximumGuestsServed", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "Lunch_MinimumGuestsServed", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "Lunch_StartTime", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "Lunch_EndTime", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "Dinner_DaysServed", c => c.String());
            AddColumn("dbo.MealSites", "Dinner_MaximumGuestsServed", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "Dinner_MinimumGuestsServed", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "Dinner_StartTime", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "Dinner_EndTime", c => c.Int(nullable: false));
            DropColumn("dbo.MealSites", "AddressLine2");
            DropColumn("dbo.MealSites", "MealServed");
            DropColumn("dbo.MealSites", "DaysServed");
            DropColumn("dbo.MealSites", "MaximumGuestsServed");
            DropColumn("dbo.MealSites", "MinimumGuestsServed");
            DropColumn("dbo.MealSites", "StartTime");
            DropColumn("dbo.MealSites", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MealSites", "EndTime", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "StartTime", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "MinimumGuestsServed", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "MaximumGuestsServed", c => c.Int(nullable: false));
            AddColumn("dbo.MealSites", "DaysServed", c => c.String());
            AddColumn("dbo.MealSites", "MealServed", c => c.String());
            AddColumn("dbo.MealSites", "AddressLine2", c => c.String());
            DropColumn("dbo.MealSites", "Dinner_EndTime");
            DropColumn("dbo.MealSites", "Dinner_StartTime");
            DropColumn("dbo.MealSites", "Dinner_MinimumGuestsServed");
            DropColumn("dbo.MealSites", "Dinner_MaximumGuestsServed");
            DropColumn("dbo.MealSites", "Dinner_DaysServed");
            DropColumn("dbo.MealSites", "Lunch_EndTime");
            DropColumn("dbo.MealSites", "Lunch_StartTime");
            DropColumn("dbo.MealSites", "Lunch_MinimumGuestsServed");
            DropColumn("dbo.MealSites", "Lunch_MaximumGuestsServed");
            DropColumn("dbo.MealSites", "Lunch_DaysServed");
            DropColumn("dbo.MealSites", "Breakfast_EndTime");
            DropColumn("dbo.MealSites", "Breakfast_StartTime");
            DropColumn("dbo.MealSites", "Breakfast_MinimumGuestsServed");
            DropColumn("dbo.MealSites", "Breakfast_MaximumGuestsServed");
            DropColumn("dbo.MealSites", "Breakfast_DaysServed");
            DropColumn("dbo.MealSites", "IsTheGatheringSite");
            DropColumn("dbo.MealSites", "Longitude");
            DropColumn("dbo.MealSites", "Latitude");
            DropColumn("dbo.MealSites", "Name");
        }
    }
}
