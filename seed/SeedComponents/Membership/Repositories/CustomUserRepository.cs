using BrockAllen.MembershipReboot.Ef;
using SeedComponents.Membership.Entities;

namespace SeedComponents.Membership.Repositories
{
	public class CustomUserRepository : DbContextUserAccountRepository<SeedDbContext, ApplicationUser>
	{
		public CustomUserRepository(SeedDbContext ctx)
			: base(ctx)
		{
		}
	}
}