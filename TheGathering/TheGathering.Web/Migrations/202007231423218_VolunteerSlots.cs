namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerSlots : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VolunteerGroupVolunteerEvents", "NumberOfGroupMembersSignedUp", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VolunteerGroupVolunteerEvents", "NumberOfGroupMembersSignedUp");
        }
    }
}
