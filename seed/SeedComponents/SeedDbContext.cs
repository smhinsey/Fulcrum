using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BrockAllen.MembershipReboot.Ef;
using SeedComponents.Membership.Entities;

namespace SeedComponents
{
	public class SeedDbContext : MembershipRebootDbContext<ApplicationUser, ApplicationGroup>
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
