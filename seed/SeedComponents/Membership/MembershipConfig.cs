using BrockAllen.MembershipReboot;
using SeedComponents.Membership.Entities;

namespace SeedComponents.Membership
{
	public class MembershipConfig : MembershipRebootConfiguration<ApplicationUser>
	{
		public static readonly MembershipConfig Config;

		static MembershipConfig()
		{
			Config = new MembershipConfig
			{
				PasswordHashingIterationCount = 10000,
				RequireAccountVerification = false
			};
		}
	}
}
