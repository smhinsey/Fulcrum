using BrockAllen.MembershipReboot.Ef;
using SeedComponents.Membership.Entities;

namespace SeedComponents.Membership.Repositories
{
	public class CustomGroupRepository : DbContextGroupRepository<SeedDbContext, ApplicationGroup>
	{
		public CustomGroupRepository(SeedDbContext ctx)
			: base(ctx)
		{
		}
	}
}