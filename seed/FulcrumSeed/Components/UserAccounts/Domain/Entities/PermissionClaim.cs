using System;

namespace FulcrumSeed.Components.UserAccounts.Domain.Entities
{
	/// <summary>
	/// Represents the basic configuration of a System.Security.Claims. Upon log in, a user's group memberships
	/// are retrieved and all PermissionClaims they contain are transformed into claims associated with the
	/// authenticated ClaimsIdentity. 
	/// </summary>
	public class PermissionClaim
	{
		public Guid Id { get; set; }

		/// <summary>
		/// System default claims cannot be deleted.
		/// </summary>
		public bool SystemDefault { get; set; }

		public string Type { get; set; }

		public string Value { get; set; }
	}
}
