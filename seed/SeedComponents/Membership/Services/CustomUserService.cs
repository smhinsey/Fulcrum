using IdentityServer3.MembershipReboot;
using SeedComponents.Membership.Entities;

namespace SeedComponents.Membership.Services
{
	public class CustomUserService : MembershipRebootUserService<ApplicationUser>
	{
		public CustomUserService(ApplicationAccountService userSvc)
			: base(userSvc)
		{
		}
	}
}