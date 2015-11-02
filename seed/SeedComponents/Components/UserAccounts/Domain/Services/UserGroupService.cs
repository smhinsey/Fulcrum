using BrockAllen.MembershipReboot;
using Fulcrum.Core.Concepts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Infrastructure.Membership;

namespace FulcrumSeed.Components.UserAccounts.Domain.Services
{
	public class UserGroupService : GroupService<UserGroup>, IDomainService
	{
		public UserGroupService(UserGroupRepository repo, MembershipConfig config)
			: base(config.DefaultTenant, repo)
		{
		}
	}
}
