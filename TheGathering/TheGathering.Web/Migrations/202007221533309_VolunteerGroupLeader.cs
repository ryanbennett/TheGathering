namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerGroupLeader : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VolunteerGroupLeaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeaderFirstName = c.String(),
                        LeaderLastName = c.String(),
                        LeaderBirthday = c.DateTime(nullable: false),
                        LeaderEmail = c.String(),
                        LeaderPhoneNumber = c.String(),
                        GroupName = c.String(),
                        SignUpForNewsLetter = c.Boolean(nullable: false),
                        ApplicationUserId = c.String(),
                        TotalGroupMembers = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.VolunteerVolunteerEvents", "VolunteerGroupLeader_Id", c => c.Int());
            CreateIndex("dbo.VolunteerVolunteerEvents", "VolunteerGroupLeader_Id");
            AddForeignKey("dbo.VolunteerVolunteerEvents", "VolunteerGroupLeader_Id", "dbo.VolunteerGroupLeaders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VolunteerVolunteerEvents", "VolunteerGroupLeader_Id", "dbo.VolunteerGroupLeaders");
            DropIndex("dbo.VolunteerVolunteerEvents", new[] { "VolunteerGroupLeader_Id" });
            DropColumn("dbo.VolunteerVolunteerEvents", "VolunteerGroupLeader_Id");
            DropTable("dbo.VolunteerGroupLeaders");
        }
    }
}
