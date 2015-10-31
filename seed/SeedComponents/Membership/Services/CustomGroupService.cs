using BrockAllen.MembershipReboot;
using SeedComponents.Membership.Entities;
using SeedComponents.Membership.Repositories;

namespace SeedComponents.Membership.Services
{
	public class CustomGroupService : GroupService<ApplicationGroup>
	{
		public CustomGroupService(CustomGroupRepository repo, MembershipConfig config)
			: base(config.DefaultTenant, repo)
		{

		}
	}
}