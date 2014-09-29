using System.Collections.Generic;
using Fulcrum.Core;
using Fulcrum.Tests.ExampleUserSystem.Commands;
using Fulcrum.Tests.ExampleUserSystem.Errors;
using Fulcrum.Tests.ExampleUserSystem.Events;
using Fulcrum.Tests.ExampleUserSystem.Queries;

namespace Fulcrum.Tests.ExampleUserSystem.CommandHandlers
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