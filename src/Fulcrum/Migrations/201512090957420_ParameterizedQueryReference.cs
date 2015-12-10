namespace Fulcrum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParameterizedQueryReference : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.IdentifierQueryReference", newName: "ParameterizedQueryReference");
            CreateTable(
                "dbo.QueryReferenceParameter",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Value = c.String(),
                        ParameterizedQueryReference_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ParameterizedQueryReference", t => t.ParameterizedQueryReference_Id)
                .Index(t => t.ParameterizedQueryReference_Id);
            
            DropColumn("dbo.ParameterizedQueryReference", "QueryParameter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ParameterizedQueryReference", "QueryParameter", c => c.String());
            DropForeignKey("dbo.QueryReferenceParameter", "ParameterizedQueryReference_Id", "dbo.ParameterizedQueryReference");
            DropIndex("dbo.QueryReferenceParameter", new[] { "ParameterizedQueryReference_Id" });
            DropTable("dbo.QueryReferenceParameter");
            RenameTable(name: "dbo.ParameterizedQueryReference", newName: "IdentifierQueryReference");
        }
    }
}
