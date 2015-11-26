using System;

namespace FulcrumSeed.Components.UserAccounts.Domain.Entities
{
	public class UserGroupAssociation
	{
		public UserRole Group { get; set; }

		public Guid GroupId { get; set; }

		public Guid Id { get; set; }

		public AppUser User { get; set; }

		public Guid UserId { get; set; }
	}
}