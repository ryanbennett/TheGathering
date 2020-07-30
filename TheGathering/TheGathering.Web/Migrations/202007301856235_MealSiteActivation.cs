namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MealSiteActivation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MealSites", "IsMealSiteActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MealSites", "IsMealSiteActive");
        }
    }
}
