namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MealSiteStartEndDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MealSites", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MealSites", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MealSites", "EndTime", c => c.Int(nullable: false));
            AlterColumn("dbo.MealSites", "StartTime", c => c.Int(nullable: false));
        }
    }
}
