using System;
using System.Collections.Generic;
using BrockAllen.MembershipReboot;

namespace FulcrumSeed.Components.UserAccounts.Domain.Entities
{
	public class UserRole : RelationalGroup
	{
		public UserRole()
		{
		}

		public UserRole(string name)
		{
			Name = name;
			Created = DateTime.UtcNow;
			LastUpdated = DateTime.UtcNow;

			// TODO: should this be configurable?
			Tenant = "default";
			ID = Guid.NewGuid();
		}

		public IList<UserGroupAssociation> AssignedUsers { get; set; }

		public virtual string Description { get; set; }

		public IList<PermissionClaim> PermissionClaims { get; set; }
	}
}
