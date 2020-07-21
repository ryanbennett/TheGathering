namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerIdLink : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VolunteerEvents", "MealSite_Id", "dbo.MealSites");
            DropIndex("dbo.VolunteerEvents", new[] { "MealSite_Id" });
            AlterColumn("dbo.VolunteerEvents", "MealSite_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.VolunteerEvents", "MealSite_Id");
            AddForeignKey("dbo.VolunteerEvents", "MealSite_Id", "dbo.MealSites", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VolunteerEvents", "MealSite_Id", "dbo.MealSites");
            DropIndex("dbo.VolunteerEvents", new[] { "MealSite_Id" });
            AlterColumn("dbo.VolunteerEvents", "MealSite_Id", c => c.Int());
            CreateIndex("dbo.VolunteerEvents", "MealSite_Id");
            AddForeignKey("dbo.VolunteerEvents", "MealSite_Id", "dbo.MealSites", "Id");
        }
    }
}
