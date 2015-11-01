using BrockAllen.MembershipReboot.Ef;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed.Components.UserAccounts.Domain.Repositories
{
	public class UserAccountRepository : DbContextUserAccountRepository<SeedDbContext, UserAccount>
	{
		public UserAccountRepository(SeedDbContext ctx)
			: base(ctx)
		{
		}
	}
}