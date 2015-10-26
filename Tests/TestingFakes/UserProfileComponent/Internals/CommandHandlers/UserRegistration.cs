using Examples.UserProfileComponent.Public.Commands;
using Examples.UserProfileComponent.Public.Queries;
using Fulcrum.Core;

namespace Examples.UserProfileComponent.Internals.CommandHandlers
{
	public class UserRegistration :
		ICommandHandler<RegisterUser>
	{
		private readonly UserQueries _userQueries;

		public UserRegistration(UserQueries userQueries)
		{
			_userQueries = userQueries;
		}

		public void Handle(RegisterUser command)
		{
			// do something useful
		}
	}
}
