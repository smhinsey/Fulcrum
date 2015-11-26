using System;

namespace FulcrumSeed.Components.UserAccounts.Queries.Projections
{
	public class RoleProjection
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public Guid RoleId { get; set; }
	}
}