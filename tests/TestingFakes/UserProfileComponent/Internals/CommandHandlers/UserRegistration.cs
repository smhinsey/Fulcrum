using Fulcrum.Core;
using TestFakes.UserProfileComponent.Public.Commands;
using TestFakes.UserProfileComponent.Public.Queries;

namespace TestFakes.UserProfileComponent.Internals.CommandHandlers
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
