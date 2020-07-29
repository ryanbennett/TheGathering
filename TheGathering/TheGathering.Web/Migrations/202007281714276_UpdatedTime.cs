namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MealSites", "Breakfast_StartTime", c => c.String());
            AlterColumn("dbo.MealSites", "Breakfast_EndTime", c => c.String());
            AlterColumn("dbo.MealSites", "Lunch_StartTime", c => c.String());
            AlterColumn("dbo.MealSites", "Lunch_EndTime", c => c.String());
            AlterColumn("dbo.MealSites", "Dinner_StartTime", c => c.String());
            AlterColumn("dbo.MealSites", "Dinner_EndTime", c => c.String());
            DropColumn("dbo.MealSites", "CrossStreet1");
            DropColumn("dbo.MealSites", "CrossStreet2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MealSites", "CrossStreet2", c => c.String());
            AddColumn("dbo.MealSites", "CrossStreet1", c => c.String());
            AlterColumn("dbo.MealSites", "Dinner_EndTime", c => c.DateTime());
            AlterColumn("dbo.MealSites", "Dinner_StartTime", c => c.DateTime());
            AlterColumn("dbo.MealSites", "Lunch_EndTime", c => c.DateTime());
            AlterColumn("dbo.MealSites", "Lunch_StartTime", c => c.DateTime());
            AlterColumn("dbo.MealSites", "Breakfast_EndTime", c => c.DateTime());
            AlterColumn("dbo.MealSites", "Breakfast_StartTime", c => c.DateTime());
        }
    }
}
