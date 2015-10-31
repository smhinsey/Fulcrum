using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace SeedComponents.Infrastructure.Identity
{
	public static class Clients
	{
		public static IEnumerable<Client> Get()
		{
			return new[]
			{
				new Client
				{
					ClientName = "FulcrumApi",
					Enabled = true,
					ClientId = "FulcrumUser",
					ClientSecrets = new List<Secret> { new Secret("secret") },
					Flow = Flows.ResourceOwner,
					AccessTokenType = AccessTokenType.Jwt,
					AccessTokenLifetime = 3600
				}
			};
		}
	}
}
