namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryingtheInt : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.VolunteerEvents", name: "Location_Id", newName: "MealSite_Id");
            RenameIndex(table: "dbo.VolunteerEvents", name: "IX_Location_Id", newName: "IX_MealSite_Id");
            AddColumn("dbo.VolunteerEvents", "Location", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VolunteerEvents", "Location");
            RenameIndex(table: "dbo.VolunteerEvents", name: "IX_MealSite_Id", newName: "IX_Location_Id");
            RenameColumn(table: "dbo.VolunteerEvents", name: "MealSite_Id", newName: "Location_Id");
        }
    }
}
