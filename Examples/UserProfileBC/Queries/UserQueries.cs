using System.Collections.Generic;
using Examples.UserProfileBC.QueryModel;
using Fulcrum.Core;

namespace Examples.UserProfileBC.Queries
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
