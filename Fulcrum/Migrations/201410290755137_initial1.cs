namespace Fulcrum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommandPublicationRecord", "ErrorDetails", c => c.String());
            AddColumn("dbo.CommandPublicationRecord", "ErrorHeadline", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommandPublicationRecord", "ErrorHeadline");
            DropColumn("dbo.CommandPublicationRecord", "ErrorDetails");
        }
    }
}
