using System;

namespace FulcrumSeed.Components.UserAccounts.Queries.Projections
{
	public class TokenProjection
	{
		public string ClientId { get; set; }

		public DateTimeOffset Expiry { get; set; }

		public string RawJson { get; set; }
	}
}