using System.ComponentModel.DataAnnotations;
using Fulcrum.Core;

namespace FulcrumSeed.Components.UserAccounts.Commands
{
	public class CreateUserRole : DefaultCommand
	{
		public string Description { get; set; }

		[Required]
		public string Name { get; set; }
	}
}
