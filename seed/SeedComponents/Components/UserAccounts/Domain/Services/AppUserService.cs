using BrockAllen.MembershipReboot;
using Fulcrum.Core.Concepts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Infrastructure.Membership;

namespace FulcrumSeed.Components.UserAccounts.Domain.Services
{
	public class AppUserService : UserAccountService<AppUser>, IDomainService
	{
		public AppUserService(MembershipConfig config, UserAccountRepository repo)
			: base(config, repo)
		{
		}
	}
}
