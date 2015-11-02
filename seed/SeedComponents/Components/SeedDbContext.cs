using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BrockAllen.MembershipReboot.Ef;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed.Components
{
	public class SeedDbContext : MembershipRebootDbContext<AppUser, UserGroup>
	{
		public SeedDbContext()
			: base("FulcrumSeedDb")
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}
