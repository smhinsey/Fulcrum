using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using FulcrumSeed.App_Packages.IdentityServer3.MembershipReboot;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Components.UserAccounts.Domain.Services;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using UserAccount = FulcrumSeed.Components.UserAccounts.Domain.Entities.UserAccount;
using UserAccountService = FulcrumSeed.Components.UserAccounts.Domain.Services.UserAccountService;

namespace FulcrumSeed.Infrastructure.Membership.Extensions
{
	public static class CustomUserServiceExtensions
	{
		public static void ConfigureCustomUserService(this IdentityServerServiceFactory factory, string connString)
		{
			factory.UserService = new Registration<IUserService, MembershipRebootUserService<UserAccount>>();

			factory.Register(new Registration<UserAccountService<UserAccount>>());
			factory.Register(new Registration<UserAccountService>());
			factory.Register(new Registration<MembershipConfig>(MembershipConfig.Config));
			factory.Register(new Registration<UserAccountRepository>());
			factory.Register(new Registration<IUserAccountRepository<UserAccount>>(r => new UserAccountRepository(new SeedDbContext())));
			factory.Register(new Registration<SeedDbContext>(resolver => new SeedDbContext()));
			factory.Register(new Registration<UserGroupService>());
			factory.Register(new Registration<DbContextUserAccountRepository<SeedDbContext, UserAccount>>());
			factory.Register(new Registration<DbContextGroupRepository<SeedDbContext, UserGroup>>());
			factory.Register(new Registration<MembershipConfig>(MembershipConfig.Config));
			factory.Register(new Registration<MembershipRebootConfiguration<UserAccount>>(new MembershipRebootConfiguration<UserAccount>()));
		}
	}
}
