using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Fulcrum.Core;
using Fulcrum.Core.Security;
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

		[RequiresClaim(ClaimTypes.Role, UserRoles.Admin)]
		public RoleProjection FindById(Guid id)
		{
			var user = _repo.GetByID(id);

			if (user != null)
			{
				var projection = new RoleProjection()
				{
					Name = user.Name,
					Description = user.Description,
					RoleId = user.ID
				};

				return projection;
			}

			return null;
		}

		[RequiresClaim(ClaimTypes.Role, UserRoles.Admin)]
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
	}
}
