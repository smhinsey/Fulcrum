using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.MembershipReboot;
using SeedComponents.Membership.Entities;
using SeedComponents.Membership.Repositories;
using SeedComponents.Membership.Services;

namespace SeedComponents.Membership.Extensions
{
	public static class CustomUserServiceExtensions
	{
		public static void ConfigureCustomUserService(this IdentityServerServiceFactory factory, string connString)
		{
			factory.UserService = new Registration<IUserService, MembershipRebootUserService<ApplicationUser>>();

			factory.Register(new Registration<UserAccountService<ApplicationUser>>());
			factory.Register(new Registration<ApplicationAccountService>());
			factory.Register(new Registration<MembershipConfig>(MembershipConfig.Config));
			factory.Register(new Registration<CustomUserRepository>());
			factory.Register(new Registration<IUserAccountRepository<ApplicationUser>>(r => new CustomUserRepository(new SeedDbContext())));
			factory.Register(new Registration<SeedDbContext>(resolver => new SeedDbContext()));
			factory.Register(new Registration<CustomGroupService>());
			factory.Register(new Registration<DbContextUserAccountRepository<SeedDbContext, ApplicationUser>>());
			factory.Register(new Registration<DbContextGroupRepository<SeedDbContext, ApplicationGroup>>());
			factory.Register(new Registration<MembershipConfig>(MembershipConfig.Config));
			factory.Register(new Registration<MembershipRebootConfiguration<ApplicationUser>>(new MembershipRebootConfiguration<ApplicationUser>()));
		}
	}
}
