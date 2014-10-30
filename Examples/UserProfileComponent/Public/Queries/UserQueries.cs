using System.Collections.Generic;
using Examples.UserProfileComponent.Public.QueryModel;
using Fulcrum.Core;

namespace Examples.UserProfileComponent.Public.Queries
{
	public class UserQueries : IQuery
	{
		public bool IsEmailAlreadyRegistered(string username)
		{
			return true;
		}

		public IList<User> ListAllUsers()
		{
			return new List<User>();
		}
	}
}
