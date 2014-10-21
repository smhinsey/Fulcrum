using Examples.UserProfileBC.Commands;
using Examples.UserProfileBC.Queries;
using Fulcrum.Core;

namespace Examples.UserProfileBC.CommandHandlers
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
