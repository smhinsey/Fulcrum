using BrockAllen.MembershipReboot;
using UserAccount = FulcrumSeed.Components.UserAccounts.Domain.Entities.UserAccount;

namespace FulcrumSeed.Infrastructure.Membership
{
	public class MembershipConfig : MembershipRebootConfiguration<UserAccount>
	{
		public static readonly MembershipConfig Config;

		static MembershipConfig()
		{
			// TODO: extract some of these settings to config
			Config = new MembershipConfig
			{
				PasswordHashingIterationCount = 10000,
				RequireAccountVerification = false,
				EmailIsUsername = true,
				EmailIsUnique = true
			};
		}
	}
}
