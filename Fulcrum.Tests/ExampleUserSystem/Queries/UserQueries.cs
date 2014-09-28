using System.Collections.Generic;
using Fulcrum.Core;
using Fulcrum.Tests.ExampleUserSystem.QueryModel;

namespace Fulcrum.Tests.ExampleUserSystem.Queries
{
  public class UserQueries : IQuery
  {
    public IList<User> ListAllUsers()
    {
      return new List<User>();
    }

		public bool IsEmailAlreadyRegistered(string username)
		{
			return true;
		}
  }
}
