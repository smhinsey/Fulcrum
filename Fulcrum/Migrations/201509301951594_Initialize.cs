namespace Fulcrum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommandPublicationRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false),
                        ErrorDetails = c.String(),
                        ErrorHeadline = c.String(),
                        Updated = c.DateTime(),
                        PortableCommand_ClrAssemblyName = c.String(),
                        PortableCommand_ClrTypeName = c.String(),
                        PortableCommand_CommandJson = c.String(),
                        PortableCommand_CommandJsonSchema = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentifierQueryReference",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        QueryParameter = c.String(),
                        QueryName = c.String(),
                        CommandPublicationRecord_Id = c.Guid(),
                        EventPublicationRecord_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommandPublicationRecord", t => t.CommandPublicationRecord_Id)
                .ForeignKey("dbo.EventPublicationRecord", t => t.EventPublicationRecord_Id)
                .Index(t => t.CommandPublicationRecord_Id)
                .Index(t => t.EventPublicationRecord_Id);
            
            CreateTable(
                "dbo.EventPublicationRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false),
                        ErrorDetails = c.String(),
                        ErrorHeadline = c.String(),
                        Updated = c.DateTime(),
                        PortableEvent_ClrAssemblyName = c.String(),
                        PortableEvent_ClrTypeName = c.String(),
                        PortableEvent_EventJson = c.String(),
                        PortableEvent_EventJsonSchema = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentifierQueryReference", "EventPublicationRecord_Id", "dbo.EventPublicationRecord");
            DropForeignKey("dbo.IdentifierQueryReference", "CommandPublicationRecord_Id", "dbo.CommandPublicationRecord");
            DropIndex("dbo.IdentifierQueryReference", new[] { "EventPublicationRecord_Id" });
            DropIndex("dbo.IdentifierQueryReference", new[] { "CommandPublicationRecord_Id" });
            DropTable("dbo.EventPublicationRecord");
            DropTable("dbo.IdentifierQueryReference");
            DropTable("dbo.CommandPublicationRecord");
        }
    }
}
