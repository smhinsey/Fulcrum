using System;
using System.ComponentModel.DataAnnotations;
using Fulcrum.Core;

namespace FulcrumSeed.Components.UserAccounts.Commands
{
	public class EditUserRole : DefaultCommand
	{
		public string Description { get; set; }

		[Required]
		public Guid RoleId { get; set; }
	}
}