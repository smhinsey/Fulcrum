using System.Collections.Generic;
using System.Security.Claims;
using BrockAllen.MembershipReboot;
using Fulcrum.Core.Concepts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Infrastructure.Membership;

namespace FulcrumSeed.Components.UserAccounts.Domain.Services
{
	public class AppUserService : UserAccountService<AppUser>, IDomainService
	{
		public AppUserService(MembershipConfig config, UserAccountRepository repo)
			: base(config, repo)
		{
		}

		public override IEnumerable<Claim> MapClaims(AppUser account)
		{
			var results = base.MapClaims(account);

			var claims = new List<Claim>(results)
			{
				new Claim("firstName", string.IsNullOrWhiteSpace(account.FirstName) ? "(First)" : account.FirstName), 
				new Claim("lastName", string.IsNullOrWhiteSpace(account.LastName) ? "(Last)" : account.LastName), 
			};

			return claims;
		}
	}
}
