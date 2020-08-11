namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerGroupVolunteerEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VolunteerGroupVolunteerEvents", "IsItCanceled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VolunteerGroupVolunteerEvents", "IsItCanceled");
        }
    }
}
