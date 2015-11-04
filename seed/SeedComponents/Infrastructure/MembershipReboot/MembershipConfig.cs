using BrockAllen.MembershipReboot;
using Fulcrum.Core.Concepts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed.Infrastructure.MembershipReboot
{
	public class MembershipConfig : MembershipRebootConfiguration<AppUser>, IDomainService
	{
		public static readonly MembershipConfig Config;

		// TODO: remove this
		// The first quick attempt failed for some reason
		static MembershipConfig()
		{
			Config = new MembershipConfig
			{
				RequireAccountVerification = false,
				EmailIsUsername = true,
				EmailIsUnique = true,
				MultiTenant = false,
				Crypto = new DefaultCrypto()
			};
		}

		public MembershipConfig()
		{
			// TODO: extract some of these settings to config
			EmailIsUsername = true;
			EmailIsUnique = true;
			MultiTenant = false;
			Crypto = new DefaultCrypto();
		}
	}
}
