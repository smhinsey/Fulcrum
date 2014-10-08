using System.Collections.Generic;
using Examples.UserProfileBC.Commands;
using Examples.UserProfileBC.Errors;
using Examples.UserProfileBC.Events;
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
      var sideEffects = new List<IEvent>();

			if (_userQueries.IsEmailAlreadyRegistered(command.EmailAddress))
			{
				throw new UsernameAlreadyInUse(command);
			}
		  
			sideEffects.Add(new UserRegistered());
    }
  }
}