namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeperateMeals2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MealSites", "Breakfast_Used", c => c.Boolean(nullable: false));
            AddColumn("dbo.MealSites", "Lunch_Used", c => c.Boolean(nullable: false));
            AddColumn("dbo.MealSites", "Dinner_Used", c => c.Boolean(nullable: false));
            AlterColumn("dbo.MealSites", "Breakfast_MaximumGuestsServed", c => c.Int());
            AlterColumn("dbo.MealSites", "Breakfast_MinimumGuestsServed", c => c.Int());
            AlterColumn("dbo.MealSites", "Breakfast_StartTime", c => c.Int());
            AlterColumn("dbo.MealSites", "Breakfast_EndTime", c => c.Int());
            AlterColumn("dbo.MealSites", "Lunch_MaximumGuestsServed", c => c.Int());
            AlterColumn("dbo.MealSites", "Lunch_MinimumGuestsServed", c => c.Int());
            AlterColumn("dbo.MealSites", "Lunch_StartTime", c => c.Int());
            AlterColumn("dbo.MealSites", "Lunch_EndTime", c => c.Int());
            AlterColumn("dbo.MealSites", "Dinner_MaximumGuestsServed", c => c.Int());
            AlterColumn("dbo.MealSites", "Dinner_MinimumGuestsServed", c => c.Int());
            AlterColumn("dbo.MealSites", "Dinner_StartTime", c => c.Int());
            AlterColumn("dbo.MealSites", "Dinner_EndTime", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MealSites", "Dinner_EndTime", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "Dinner_StartTime", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "Dinner_MinimumGuestsServed", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "Dinner_MaximumGuestsServed", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "Lunch_EndTime", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "Lunch_StartTime", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "Lunch_MinimumGuestsServed", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "Lunch_MaximumGuestsServed", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "Breakfast_EndTime", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "Breakfast_StartTime", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "Breakfast_MinimumGuestsServed", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "Breakfast_MaximumGuestsServed", c => c.Int(nullable: false));
            DropColumn("dbo.MealSites", "Dinner_Used");
            DropColumn("dbo.MealSites", "Lunch_Used");
            DropColumn("dbo.MealSites", "Breakfast_Used");
        }
    }
}
