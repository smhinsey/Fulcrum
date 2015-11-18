using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Fulcrum.Core;
using Fulcrum.Core.Security;

namespace FulcrumSeed.Components.UserAccounts.Commands
{
	[RequiresClaim(ClaimTypes.Role, UserRoles.Admin)]
	public class EditAccount : DefaultCommand
	{
		[Required]
		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public Guid Id { get; set; }

		//public Guid[] GroupIds { get; set; }
	}
}
