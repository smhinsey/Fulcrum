using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using FulcrumSeed.Components;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Components.UserAccounts.Domain.Services;
using IdentityManager;
using IdentityManager.Configuration;
using IdentityManager.MembershipReboot;

namespace FulcrumSeed.Infrastructure.MembershipReboot.Extensions
{
	// TODO: get rid of this
	public static class MembershipRebootIdentityManagerServiceExtensions
	{
		public static void Configure(this IdentityManagerServiceFactory factory, string connectionString)
		{
			factory.IdentityManagerService =
				new Registration<IIdentityManagerService, MembershipRebootIdentityManagerService<AppUser, UserClaimGroup>>();
			factory.Register(new Registration<UserAccountService<AppUser>>());
			factory.Register(new Registration<UserGroupService>());
			factory.Register(new Registration<DbContextUserAccountRepository<SeedDbContext, AppUser>>());
			factory.Register(new Registration<DbContextGroupRepository<SeedDbContext, UserClaimGroup>>());
			factory.Register(new Registration<SeedDbContext>(resolver => new SeedDbContext()));
			factory.Register(new Registration<MembershipConfig>(MembershipConfig.Config));
			factory.Register(new Registration<UserAccountRepository>());
			factory.Register(new Registration<IUserAccountRepository<AppUser>>(r => new UserAccountRepository(new SeedDbContext())));
			factory.Register(new Registration<MembershipRebootConfiguration<AppUser>>(new MembershipRebootConfiguration<AppUser>()));
		}
	}
}
