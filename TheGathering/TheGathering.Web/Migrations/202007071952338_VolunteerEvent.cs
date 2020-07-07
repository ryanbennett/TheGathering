namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerEvent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VolunteerEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartingShiftTime = c.DateTime(nullable: false),
                        ShiftSpan = c.Time(nullable: false, precision: 7),
                        OpenSlots = c.Int(nullable: false),
                        Location = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VolunteerEvents");
        }
    }
}
