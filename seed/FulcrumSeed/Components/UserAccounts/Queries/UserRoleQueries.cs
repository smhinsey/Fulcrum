using System.Collections.Generic;
using System.Linq;
using Fulcrum.Core;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Components.UserAccounts.Queries.Projections;

namespace FulcrumSeed.Components.UserAccounts.Queries
{
	public class UserRoleQueries : IQuery
	{
		private readonly UserGroupRepository _repo;

		public UserRoleQueries(UserGroupRepository repo)
		{
			_repo = repo;
		}

		// TODO: enable this
		//[RequiresClaim(ClaimTypes.Role, UserRoles.Admin)]
		public IList<RoleProjection> ListRoles()
		{
			var users = _repo.ListAll();

			var projection = users.Select(a => new RoleProjection()
			{
				Name = a.Name,
				Description = a.Description,
				RoleId = a.ID
			});

			return projection.ToList();
		}

		public bool Test(string param1, int param2, bool param3, string param4)
		{
			return false;
		}
	}
}