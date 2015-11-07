namespace FulcrumSeed.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserClaimGroup2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PermissionClaim", "SystemDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PermissionClaim", "SystemDefault", c => c.String());
        }
    }
}
