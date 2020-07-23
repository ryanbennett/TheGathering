namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DaysOfTheWeek : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MealSites", "Breakfast_DaysServed");
            DropColumn("dbo.MealSites", "Lunch_DaysServed");
            DropColumn("dbo.MealSites", "Dinner_DaysServed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MealSites", "Dinner_DaysServed", c => c.String());
            AddColumn("dbo.MealSites", "Lunch_DaysServed", c => c.String());
            AddColumn("dbo.MealSites", "Breakfast_DaysServed", c => c.String());
        }
    }
}
