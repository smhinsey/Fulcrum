using BrockAllen.MembershipReboot;
using SeedComponents.Membership.Entities;
using SeedComponents.Membership.Repositories;

namespace SeedComponents.Membership.Services
{
	public class ApplicationAccountService : UserAccountService<ApplicationUser>
	{
		public ApplicationAccountService(MembershipConfig config, CustomUserRepository repo)
			: base(config, repo)
		{
		}
	}
}