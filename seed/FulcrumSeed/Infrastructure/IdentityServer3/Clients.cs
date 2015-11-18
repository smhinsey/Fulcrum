using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core.Models;

namespace FulcrumSeed.Infrastructure.IdentityServer3
{
	public static class Clients
	{
		public static IEnumerable<Client> Get()
		{
			// TODO: make configurable
			var clientName = "FulcrumApi";

			// TODO: make configurable
			var clientId = "FulcrumApi";

			// TODO: make configurable
			var apiSecret = "apiSecret";

			// TODO: make configurable
			var accessTokenLifetime = 3600;

			return new[]
			{
				new Client
				{
					ClientName = clientName,
					Enabled = true,
					ClientId = clientId,
					ClientSecrets = new List<Secret>()
					{
						new Secret(apiSecret.Sha256())
					},
					Flow = Flows.ResourceOwner,
					AccessTokenType = AccessTokenType.Jwt,
					AccessTokenLifetime = accessTokenLifetime,
					AllowAccessToAllScopes = true,
					AllowAccessToAllCustomGrantTypes = true,
					AlwaysSendClientClaims = true,
					AllowedScopes = new List<string>()
					{
						"openid",
						"profile",
						"roles",
						"FulcrumApiScope",
						"firstName",
						"lastName",
						"name",
						"read",
						"write",
					},
					UpdateAccessTokenClaimsOnRefresh = true,
					IncludeJwtId = true,
					PrefixClientClaims = false,
					Claims = new List<Claim>(){new Claim("PrefixTest","TestPrefix")}
				}
			};
		}
	}
}
