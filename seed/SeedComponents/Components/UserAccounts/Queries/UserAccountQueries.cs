using System;
using System.Collections.Generic;
using System.Linq;
using Fulcrum.Core;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;

namespace FulcrumSeed.Components.UserAccounts.Queries
{
	public class UserAccountQueries : IQuery
	{
		private readonly UserAccountRepository _repo;

		public UserAccountQueries(UserAccountRepository repo)
		{
			_repo = repo;
		}

		public IList<AccountProjection> ListUsers()
		{
			var users = _repo.ListAll();

			var projection = users.Select(a => new AccountProjection()
			{
				FirstName = a.FirstName,
				LastName = a.LastName,
				Email = a.Email,
				Id = a.ID
			});

			return projection.ToList();
		}
	}

	public class AccountProjection
	{
		public string Email { get; set; }

		public string FirstName { get; set; }

		public Guid Id { get; set; }

		public string LastName { get; set; }
	}
}
