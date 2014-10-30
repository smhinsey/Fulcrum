using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Fulcrum.Runtime.CommandPipeline
{
	public class CommandPipelineDbContext : DbContext
	{
		public CommandPipelineDbContext()
			: base("CommandPipelineDbContext")
		{
		}

		public DbSet<CommandPublicationRecord> CommandPublicationRecords { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}
