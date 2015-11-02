using Fulcrum.Core;

namespace FulcrumSeed.Components.UserAccounts.Commands
{
	public class RegisterAccount : DefaultCommand
	{
		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Password { get; set; }

		public string PasswordConfirm { get; set; }
	}
}
