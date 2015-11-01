using BrockAllen.MembershipReboot;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Infrastructure.Membership;

namespace FulcrumSeed.Components.UserAccounts.Domain.Services
{
	public class UserGroupService : GroupService<UserGroup>
	{
		public UserGroupService(UserGroupRepository repo, MembershipConfig config)
			: base(config.DefaultTenant, repo)
		{
		}
	}
}
