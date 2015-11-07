using System;

namespace FulcrumSeed.Components.UserAccounts.Queries.Projections
{
	public class GroupProjection
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public Guid Id { get; set; }
	}
}