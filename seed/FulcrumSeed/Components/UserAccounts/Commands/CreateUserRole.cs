using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Fulcrum.Core;
using Fulcrum.Core.Security;

namespace FulcrumSeed.Components.UserAccounts.Commands
{
	[RequiresClaim(ClaimTypes.Role, UserRoles.Admin)]
	public class CreateUserRole : DefaultCommand
	{
		public string Description { get; set; }

		[Required]
		public string Name { get; set; }
	}
}
