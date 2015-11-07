namespace FulcrumSeed.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserClaimGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserGroupAssociation",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Group_Key = c.Int(),
                        User_Key = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_Key)
                .ForeignKey("dbo.UserAccounts", t => t.User_Key)
                .Index(t => t.Group_Key)
                .Index(t => t.User_Key);
            
            CreateTable(
                "dbo.PermissionClaim",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(),
                        Value = c.String(),
                        UserClaimGroup_Key = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.UserClaimGroup_Key)
                .Index(t => t.UserClaimGroup_Key);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PermissionClaim", "UserClaimGroup_Key", "dbo.Groups");
            DropForeignKey("dbo.UserGroupAssociation", "User_Key", "dbo.UserAccounts");
            DropForeignKey("dbo.UserGroupAssociation", "Group_Key", "dbo.Groups");
            DropIndex("dbo.PermissionClaim", new[] { "UserClaimGroup_Key" });
            DropIndex("dbo.UserGroupAssociation", new[] { "User_Key" });
            DropIndex("dbo.UserGroupAssociation", new[] { "Group_Key" });
            DropTable("dbo.PermissionClaim");
            DropTable("dbo.UserGroupAssociation");
        }
    }
}
