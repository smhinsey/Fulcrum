using System.Data.Entity.Migrations;
using Fulcrum.Runtime.CommandPipeline;

namespace Fulcrum.Migrations
{
	public sealed class Configuration : DbMigrationsConfiguration<CommandPipelineDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			ContextKey = "Fulcrum.Runtime.CommandPipeline";
		}

		protected override void Seed(CommandPipelineDbContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//
		}
	}
}
