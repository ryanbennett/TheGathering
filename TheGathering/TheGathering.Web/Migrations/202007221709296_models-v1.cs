namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelsv1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MealSites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AddressLine1 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zipcode = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        CrossStreet1 = c.String(),
                        CrossStreet2 = c.String(),
                        IsTheGatheringSite = c.Boolean(nullable: false),
                        Breakfast_Used = c.Boolean(nullable: false),
                        Breakfast_DaysServed = c.String(),
                        Breakfast_MaximumGuestsServed = c.Int(),
                        Breakfast_MinimumGuestsServed = c.Int(),
                        Breakfast_StartTime = c.DateTime(),
                        Breakfast_EndTime = c.DateTime(),
                        Lunch_Used = c.Boolean(nullable: false),
                        Lunch_DaysServed = c.String(),
                        Lunch_MaximumGuestsServed = c.Int(),
                        Lunch_MinimumGuestsServed = c.Int(),
                        Lunch_StartTime = c.DateTime(),
                        Lunch_EndTime = c.DateTime(),
                        Dinner_Used = c.Boolean(nullable: false),
                        Dinner_DaysServed = c.String(),
                        Dinner_MaximumGuestsServed = c.Int(),
                        Dinner_MinimumGuestsServed = c.Int(),
                        Dinner_StartTime = c.DateTime(),
                        Dinner_EndTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VolunteerEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartingShiftTime = c.DateTime(nullable: false),
                        EndingShiftTime = c.DateTime(nullable: false),
                        OpenSlots = c.Int(nullable: false),
                        MealSite_Id = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MealSites", t => t.MealSite_Id, cascadeDelete: true)
                .Index(t => t.MealSite_Id);
            
            CreateTable(
                "dbo.VolunteerVolunteerEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VolunteerId = c.Int(nullable: false),
                        VolunteerEventId = c.Int(nullable: false),
                        Confirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VolunteerEvents", t => t.VolunteerEventId, cascadeDelete: true)
                .ForeignKey("dbo.Volunteers", t => t.VolunteerId, cascadeDelete: true)
                .Index(t => t.VolunteerId)
                .Index(t => t.VolunteerEventId);
            
            CreateTable(
                "dbo.Volunteers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Birthday = c.DateTime(nullable: false),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        InterestInLeadership = c.Boolean(nullable: false),
                        SignUpForNewsLetter = c.Boolean(nullable: false),
                        ApplicationUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VolunteerVolunteerEvents", "VolunteerId", "dbo.Volunteers");
            DropForeignKey("dbo.VolunteerVolunteerEvents", "VolunteerEventId", "dbo.VolunteerEvents");
            DropForeignKey("dbo.VolunteerEvents", "MealSite_Id", "dbo.MealSites");
            DropIndex("dbo.VolunteerVolunteerEvents", new[] { "VolunteerEventId" });
            DropIndex("dbo.VolunteerVolunteerEvents", new[] { "VolunteerId" });
            DropIndex("dbo.VolunteerEvents", new[] { "MealSite_Id" });
            DropTable("dbo.Volunteers");
            DropTable("dbo.VolunteerVolunteerEvents");
            DropTable("dbo.VolunteerEvents");
            DropTable("dbo.MealSites");
        }
    }
}
