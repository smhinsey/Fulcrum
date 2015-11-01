using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace FulcrumSeed.Infrastructure.Identity
{
	public static class Scopes
	{
		public static IEnumerable<Scope> Get()
		{
			var scopes = new List<Scope>
			{
				new Scope
				{
					Enabled = true,
					Name = "FulcrumApiScope",
					Description = "Grants access to the Fulcrum API.",
					Type = ScopeType.Resource
				}
			};

			scopes.AddRange(StandardScopes.All);

			return scopes;
		}
	}
}
