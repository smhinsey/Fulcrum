namespace Fulcrum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommandPublicationRecord", "CreationTimestamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.CommandPublicationRecord", "ModificationTimestamp", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommandPublicationRecord", "ModificationTimestamp");
            DropColumn("dbo.CommandPublicationRecord", "CreationTimestamp");
        }
    }
}
