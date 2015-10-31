using BrockAllen.MembershipReboot.Ef;
using SeedComponents.Membership.Entities;

namespace SeedComponents.Membership.Repositories
{
	public class CustomGroupRepository : DbContextGroupRepository<MembershipDbContext, ApplicationGroup>
	{
		public CustomGroupRepository(MembershipDbContext ctx)
			: base(ctx)
		{
		}
	}
}