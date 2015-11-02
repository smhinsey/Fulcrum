using Fulcrum.Core.Concepts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Infrastructure.Membership;

namespace FulcrumSeed.Components.UserAccounts.Domain.Services
{
	public class UserAccountService : BrockAllen.MembershipReboot.UserAccountService<UserAccount>, IDomainService
	{
		public UserAccountService(MembershipConfig config, UserAccountRepository repo)
			: base(config, repo)
		{
		}
	}
}
