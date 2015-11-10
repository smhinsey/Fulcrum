using System.Collections.Generic;
using System.Linq;
using Fulcrum.Core;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Components.UserAccounts.Queries.Projections;

namespace FulcrumSeed.Components.UserAccounts.Queries
{
	public class UserGroupQueries : IQuery
	{
		private readonly UserGroupRepository _repo;

		public UserGroupQueries(UserGroupRepository repo)
		{
			_repo = repo;
		}

		// TODO: enable this
		//[RequiresClaim(ClaimTypes.Role, UserRoles.Admin)]
		public IList<GroupProjection> ListGroups()
		{
			var users = _repo.ListAll();

			var projection = users.Select(a => new GroupProjection()
			{
				GroupName = a.Name,
				Description = a.Description,
				Id = a.ID
			});

			return projection.ToList();
		}
	}
}