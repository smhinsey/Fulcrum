namespace SeedComponents.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BasicProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAccounts", "FirstName", c => c.String());
            AddColumn("dbo.UserAccounts", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAccounts", "LastName");
            DropColumn("dbo.UserAccounts", "FirstName");
        }
    }
}
