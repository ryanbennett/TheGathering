namespace TheGathering.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountDeactivation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Volunteers", "IsAccountActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Volunteers", "IsAccountActive");
        }
    }
}
