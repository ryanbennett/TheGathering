namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VolunteerEvent1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VolunteerEvents", "EndingShiftTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.VolunteerEvents", "ShiftSpan");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VolunteerEvents", "ShiftSpan", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.VolunteerEvents", "EndingShiftTime");
        }
    }
}
