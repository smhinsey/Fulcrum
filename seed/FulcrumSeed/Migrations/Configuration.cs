using System.Data.Entity.Migrations;
using System.Security.Claims;
using BrockAllen.MembershipReboot;
using FulcrumSeed.Components;
using FulcrumSeed.Components.UserAccounts;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Components.UserAccounts.Domain.Services;
using FulcrumSeed.Infrastructure.MembershipReboot;

namespace FulcrumSeed.Migrations
{
	public sealed class Configuration : DbMigrationsConfiguration<SeedDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(SeedDbContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//

			var svc = new AppUserService(MembershipConfig.Config, new UserAccountRepository(new SeedDbContext()));

			var user = svc.GetByUsername("testAdmin@example.com");

			if (user == null)
			{
				var admin = svc.CreateAccount("testAdmin@example.com", "password", "testAdmin@example.com");

				svc.AddClaim(admin.ID, ClaimTypes.Role, UserRoles.Admin);
				svc.AddClaim(admin.ID, ClaimTypes.Role, UserRoles.AuthenticatedUser);
			}
			else
			{
				var claims = svc.MapClaims(user);

				svc.RemoveClaims(user.ID, new UserClaimCollection(claims));

				svc.AddClaim(user.ID, ClaimTypes.Role, UserRoles.Admin);
				svc.AddClaim(user.ID, ClaimTypes.Role, UserRoles.AuthenticatedUser);
			}
		}
	}
}
