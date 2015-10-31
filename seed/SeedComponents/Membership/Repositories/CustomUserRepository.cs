using BrockAllen.MembershipReboot.Ef;
using SeedComponents.Membership.Entities;

namespace SeedComponents.Membership.Repositories
{
	public class CustomUserRepository : DbContextUserAccountRepository<MembershipDbContext, ApplicationUser>
	{
		public CustomUserRepository(MembershipDbContext ctx)
			: base(ctx)
		{
		}
	}
}