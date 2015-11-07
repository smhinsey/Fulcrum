namespace FulcrumSeed.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserClaimGroup1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PermissionClaim", "SystemDefault", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PermissionClaim", "SystemDefault");
        }
    }
}
