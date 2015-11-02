using BrockAllen.MembershipReboot.Ef;
using Fulcrum.Core.Concepts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed.Components.UserAccounts.Domain.Repositories
{
	public class UserAccountRepository : DbContextUserAccountRepository<SeedDbContext, UserAccount>, IRepository
	{
		public UserAccountRepository(SeedDbContext ctx)
			: base(ctx)
		{
		}
	}
}