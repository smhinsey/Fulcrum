using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Fulcrum.Core;
using Fulcrum.Core.Security;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Components.UserAccounts.Queries.Projections;

namespace FulcrumSeed.Components.UserAccounts.Queries
{
	public class UserAccountQueries : IQuery
	{
		private readonly UserAccountRepository _repo;

		public UserAccountQueries(UserAccountRepository repo)
		{
			_repo = repo;
		}

		[RequiresClaim(ClaimTypes.Role, UserRoles.Admin)]
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
}
