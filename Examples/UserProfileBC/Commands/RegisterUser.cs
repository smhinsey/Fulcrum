using Fulcrum.Core;

namespace Examples.UserProfileBC.Commands
{
	public class RegisterUser : ICommand
	{
		public string DisplayName { get; set; }

		public string EmailAddress { get; set; }

		public string PasswordHash { get; set; }
	}
}
