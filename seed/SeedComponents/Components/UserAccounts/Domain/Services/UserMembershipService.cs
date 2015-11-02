using Fulcrum.Core.Concepts;
using FulcrumSeed.App_Packages.IdentityServer3.MembershipReboot;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed.Components.UserAccounts.Domain.Services
{
	public class UserMembershipService : MembershipRebootUserService<UserAccount>, IDomainService
	{
		public UserMembershipService(UserAccountService userSvc)
			: base(userSvc)
		{
		}
	}
}