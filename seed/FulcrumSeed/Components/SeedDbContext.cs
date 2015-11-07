using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BrockAllen.MembershipReboot.Ef;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed.Components
{
	public class SeedDbContext : MembershipRebootDbContext<AppUser, UserClaimGroup>
	{
		public SeedDbContext()
			: base("FulcrumSeedDb")
		{
			this.RegisterUserAccountChildTablesForDelete<AppUser>();
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}
