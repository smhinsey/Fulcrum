using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Services;
using IdentityManager;
using IdentityManager.Configuration;
using IdentityManager.MembershipReboot;
using UserAccount = FulcrumSeed.Components.UserAccounts.Domain.Entities.UserAccount;

namespace FulcrumSeed.Infrastructure.Membership.Extensions
{
	public static class MembershipRebootIdentityManagerServiceExtensions
	{
		public static void Configure(this IdentityManagerServiceFactory factory, string connectionString)
		{
			factory.IdentityManagerService =
				new Registration<IIdentityManagerService, MembershipRebootIdentityManagerService<UserAccount, UserGroup>>();
			factory.Register(new Registration<UserAccountService<UserAccount>>());
			factory.Register(new Registration<UserGroupService>());
			factory.Register(new Registration<DbContextUserAccountRepository<SeedDbContext, UserAccount>>());
			factory.Register(new Registration<DbContextGroupRepository<SeedDbContext, UserGroup>>());
			factory.Register(new Registration<SeedDbContext>(resolver => new SeedDbContext()));
			factory.Register(new Registration<MembershipConfig>(MembershipConfig.Config));
		}
	}
}
