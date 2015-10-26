namespace Fulcrum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommandPublicationRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PortableCommand_ClrAssemblyName = c.String(),
                        PortableCommand_ClrTypeName = c.String(),
                        PortableCommand_CommandJson = c.String(),
                        PortableCommand_CommandJsonSchema = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CommandPublicationRecord");
        }
    }
}
