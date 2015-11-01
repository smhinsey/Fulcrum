using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BrockAllen.MembershipReboot.Ef;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed
{
	public class SeedDbContext : MembershipRebootDbContext<UserAccount, UserGroup>
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
