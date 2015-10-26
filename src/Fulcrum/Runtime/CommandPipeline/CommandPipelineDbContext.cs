using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Fulcrum.Core;
using Fulcrum.Runtime.EventPipeline;

namespace Fulcrum.Runtime.CommandPipeline
{
	public class CommandPipelineDbContext : DbContext
	{
		public CommandPipelineDbContext()
			: base("CommandPipelineDbContext")
		{
		}

		public DbSet<CommandPublicationRecord> CommandPublicationRecords { get; set; }

		public DbSet<IdentifierQueryReference> QueryReferences { get; set; }

        public DbSet<EventPublicationRecord> EventPublicationRecords { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}
