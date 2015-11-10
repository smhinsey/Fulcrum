using System;
using System.ComponentModel.DataAnnotations;
using Fulcrum.Core;

namespace FulcrumSeed.Components.UserAccounts.Commands
{
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
