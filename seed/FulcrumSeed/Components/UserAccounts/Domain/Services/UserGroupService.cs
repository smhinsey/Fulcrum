using BrockAllen.MembershipReboot;
using Fulcrum.Core.Concepts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Infrastructure.MembershipReboot;

namespace FulcrumSeed.Components.UserAccounts.Domain.Services
{
	public class UserGroupService : GroupService<UserRole>, IDomainService
	{
		public UserGroupService(UserGroupRepository repo, MembershipConfig config)
			: base(config.DefaultTenant, repo)
		{
		}
	}
}
