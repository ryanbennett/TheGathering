namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerGroupLeaderChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VolunteerGroupLeaders", "LeaderFirstName", c => c.String(maxLength: 60));
            AlterColumn("dbo.VolunteerGroupLeaders", "LeaderLastName", c => c.String(maxLength: 60));
            AlterColumn("dbo.VolunteerGroupLeaders", "LeaderPhoneNumber", c => c.String(maxLength: 13));
            AlterColumn("dbo.VolunteerGroupLeaders", "GroupName", c => c.String(maxLength: 60));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VolunteerGroupLeaders", "GroupName", c => c.String());
            AlterColumn("dbo.VolunteerGroupLeaders", "LeaderPhoneNumber", c => c.String());
            AlterColumn("dbo.VolunteerGroupLeaders", "LeaderLastName", c => c.String());
            AlterColumn("dbo.VolunteerGroupLeaders", "LeaderFirstName", c => c.String());
        }
    }
}
