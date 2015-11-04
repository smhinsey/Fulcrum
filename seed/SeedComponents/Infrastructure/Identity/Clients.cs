using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace FulcrumSeed.Infrastructure.Identity
{
	public static class Clients
	{
		public static IEnumerable<Client> Get()
		{
			return new[]
			{
				// TODO: make ClientName configurable
				// TODO: make ClientId configurable
				// TODO: make ClientSecrets configurable
				// TODO: make AccessTokenLifetime configurable
				new Client
				{
					ClientName = "FulcrumApi",
					Enabled = true,
					ClientId = "FulcrumApi",
					ClientSecrets = new List<Secret>()
					{
						new Secret("apiSecret".Sha256())
					},
					Flow = Flows.ResourceOwner,
					AccessTokenType = AccessTokenType.Jwt,
					AccessTokenLifetime = 3600,
					AllowedScopes = new List<string>()
					{
						"openid",
						"profile",
						"roles",
						"FulcrumApiScope"
					},
					UpdateAccessTokenClaimsOnRefresh = true,
				}
			};
		}
	}
}
