using Fulcrum.Core;

namespace Examples.UserProfileComponent.Public.Commands
{
	public class RegisterUser : DefaultCommand
	{
		public string DisplayName { get; set; }

		public string EmailAddress { get; set; }

		public string PasswordHash { get; set; }
	}
}
