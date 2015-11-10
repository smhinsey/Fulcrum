using System;

namespace FulcrumSeed.Components.UserAccounts.Queries.Projections
{
	public class AccountProjection
	{
		public string Email { get; set; }

		public string FirstName { get; set; }

		public Guid[] GroupIds { get; set; }

		public Guid Id { get; set; }

		public string LastName { get; set; }
	}
}
