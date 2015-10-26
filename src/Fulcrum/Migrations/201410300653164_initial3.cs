namespace Fulcrum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommandPublicationRecord", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.CommandPublicationRecord", "Updated", c => c.DateTime());
            DropColumn("dbo.CommandPublicationRecord", "CreationTimestamp");
            DropColumn("dbo.CommandPublicationRecord", "ModificationTimestamp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CommandPublicationRecord", "ModificationTimestamp", c => c.DateTime());
            AddColumn("dbo.CommandPublicationRecord", "CreationTimestamp", c => c.DateTime(nullable: false));
            DropColumn("dbo.CommandPublicationRecord", "Updated");
            DropColumn("dbo.CommandPublicationRecord", "Created");
        }
    }
}
