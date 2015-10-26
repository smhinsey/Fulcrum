using System.Collections.Generic;
using Fulcrum.Core;
using TestFakes.UserProfileComponent.Public.QueryModel;

namespace TestFakes.UserProfileComponent.Public.Queries
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
