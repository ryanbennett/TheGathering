namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MealSite : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MealSites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zipcode = c.String(),
                        CrossStreet1 = c.String(),
                        CrossStreet2 = c.String(),
                        MealServed = c.String(),
                        DaysServed = c.String(),
                        MaximumGuestsServed = c.Int(nullable: false),
                        MinimumGuestsServed = c.Int(nullable: false),
                        StartTime = c.Int(nullable: false),
                        EndTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MealSites");
        }
    }
}
