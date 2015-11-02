using BrockAllen.MembershipReboot;
using Fulcrum.Core.Concepts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed.Infrastructure.Membership
{
	public class MembershipConfig : MembershipRebootConfiguration<AppUser>, IDomainService
	{
		public static readonly MembershipConfig Config;

		static MembershipConfig()
		{
			// TODO: extract some of these settings to config
			Config = new MembershipConfig
			{
				RequireAccountVerification = false,
				EmailIsUsername = true,
				EmailIsUnique = true,
				MultiTenant = false,
				Crypto = new DefaultCrypto()
			};
		}
	}
}
