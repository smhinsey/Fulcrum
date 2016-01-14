using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Fulcrum.Core;
using Fulcrum.Core.Security;

namespace FulcrumSeed.Components.UserAccounts.Commands
{
	[RequiresClaim(ClaimTypes.Role, UserRoles.Admin)]
	public class EditUserRole : DefaultCommand
	{
		public string Description { get; set; }

		[Required]
		public Guid RoleId { get; set; }
	}
}