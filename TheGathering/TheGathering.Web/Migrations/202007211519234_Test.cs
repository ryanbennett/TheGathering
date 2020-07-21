namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VolunteerVolunteerEvents", "VolunteerId", "dbo.Volunteers");
            DropForeignKey("dbo.VolunteerVolunteerEvents", "VolunteerEventId", "dbo.VolunteerEvents");
            DropIndex("dbo.VolunteerVolunteerEvents", new[] { "VolunteerEventId" });
            DropIndex("dbo.VolunteerVolunteerEvents", new[] { "VolunteerId" });
            DropTable("dbo.VolunteerVolunteerEvents");
        }
    }
}
