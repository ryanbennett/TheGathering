namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerGroupVolunteerEvent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VolunteerVolunteerEvents", "VolunteerGroupLeader_Id", "dbo.VolunteerGroupLeaders");
            DropIndex("dbo.VolunteerVolunteerEvents", new[] { "VolunteerGroupLeader_Id" });
            CreateTable(
                "dbo.VolunteerGroupVolunteerEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VolunteerGroupId = c.Int(nullable: false),
                        VolunteerEventId = c.Int(nullable: false),
                        Confirmed = c.Boolean(nullable: false),
                        VolunteerGroupLeader_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VolunteerEvents", t => t.VolunteerEventId, cascadeDelete: true)
                .ForeignKey("dbo.VolunteerGroupLeaders", t => t.VolunteerGroupLeader_Id)
                .Index(t => t.VolunteerEventId)
                .Index(t => t.VolunteerGroupLeader_Id);
            
            DropColumn("dbo.VolunteerVolunteerEvents", "VolunteerGroupLeader_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VolunteerVolunteerEvents", "VolunteerGroupLeader_Id", c => c.Int());
            DropForeignKey("dbo.VolunteerGroupVolunteerEvents", "VolunteerGroupLeader_Id", "dbo.VolunteerGroupLeaders");
            DropForeignKey("dbo.VolunteerGroupVolunteerEvents", "VolunteerEventId", "dbo.VolunteerEvents");
            DropIndex("dbo.VolunteerGroupVolunteerEvents", new[] { "VolunteerGroupLeader_Id" });
            DropIndex("dbo.VolunteerGroupVolunteerEvents", new[] { "VolunteerEventId" });
            DropTable("dbo.VolunteerGroupVolunteerEvents");
            CreateIndex("dbo.VolunteerVolunteerEvents", "VolunteerGroupLeader_Id");
            AddForeignKey("dbo.VolunteerVolunteerEvents", "VolunteerGroupLeader_Id", "dbo.VolunteerGroupLeaders", "Id");
        }
    }
}
