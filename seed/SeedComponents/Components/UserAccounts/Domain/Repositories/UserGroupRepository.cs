using BrockAllen.MembershipReboot.Ef;
using Fulcrum.Core.Concepts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed.Components.UserAccounts.Domain.Repositories
{
	public class UserGroupRepository : DbContextGroupRepository<SeedDbContext, UserGroup>, IRepository
	{
		public UserGroupRepository(SeedDbContext ctx)
			: base(ctx)
		{
		}
	}
}