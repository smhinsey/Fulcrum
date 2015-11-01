using BrockAllen.MembershipReboot.Ef;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed.Components.UserAccounts.Domain.Repositories
{
	public class UserGroupRepository : DbContextGroupRepository<SeedDbContext, UserGroup>
	{
		public UserGroupRepository(SeedDbContext ctx)
			: base(ctx)
		{
		}
	}
}