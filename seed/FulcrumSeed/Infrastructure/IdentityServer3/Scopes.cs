using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace FulcrumSeed.Infrastructure.IdentityServer3
{
	public static class Scopes
	{
		public static IEnumerable<Scope> Get()
		{
			var scopes = new List<Scope>
			{
				// TODO: we might want to lock down the claims here
				// one option would be to enumerate the list and remove
				// anything with an http:// prefix in the type
				new Scope
				{
					Enabled = true,
					Name = "FulcrumApiScope",
					Description = "Grants access to the Fulcrum API.",
					Type = ScopeType.Identity,
					IncludeAllClaimsForUser = true,
					Claims = new List<ScopeClaim>
					{
						//new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.Email),
						//new ScopeClaim(System.Security.Claims.ClaimTypes.Role),
						//new ScopeClaim("firstName"),
						//new ScopeClaim("fastName"),
					}
				}
			};

			scopes.AddRange(StandardScopes.All);

			return scopes;
		}
	}
}
