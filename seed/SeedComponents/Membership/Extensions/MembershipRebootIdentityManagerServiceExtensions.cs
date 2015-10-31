using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using IdentityManager;
using IdentityManager.Configuration;
using IdentityManager.MembershipReboot;
using SeedComponents.Membership.Entities;
using SeedComponents.Membership.Services;

namespace SeedComponents.Membership.Extensions
{
	public static class MembershipRebootIdentityManagerServiceExtensions
	{
		public static void Configure(this IdentityManagerServiceFactory factory, string connectionString)
		{
			factory.IdentityManagerService =
				new Registration<IIdentityManagerService, MembershipRebootIdentityManagerService<ApplicationUser, ApplicationGroup>>();
			factory.Register(new Registration<UserAccountService<ApplicationUser>>());
			factory.Register(new Registration<CustomGroupService>());
			factory.Register(new Registration<DbContextUserAccountRepository<MembershipDbContext, ApplicationUser>>());
			factory.Register(new Registration<DbContextGroupRepository<MembershipDbContext, ApplicationGroup>>());
			factory.Register(new Registration<MembershipDbContext>(resolver => new MembershipDbContext(connectionString)));
			factory.Register(new Registration<MembershipConfig>(MembershipConfig.Config));
		}
	}
}
