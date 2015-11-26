namespace FulcrumSeed.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renaming : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PermissionClaim", name: "UserClaimGroup_Key", newName: "UserRole_Key");
            RenameIndex(table: "dbo.PermissionClaim", name: "IX_UserClaimGroup_Key", newName: "IX_UserRole_Key");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PermissionClaim", name: "IX_UserRole_Key", newName: "IX_UserClaimGroup_Key");
            RenameColumn(table: "dbo.PermissionClaim", name: "UserRole_Key", newName: "UserClaimGroup_Key");
        }
    }
}
