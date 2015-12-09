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

		public IList<RoleProjection> Test(string param1, int param2, bool param3, string param4)
		{
			return new List<RoleProjection>
			{
				new RoleProjection { Description = "Lorem ipsum", Name = "SampleRole", RoleId = Guid.NewGuid() },
				new RoleProjection { Description = "Dolor sit amet", Name = "AnotherSampleRole", RoleId = Guid.NewGuid() }
			};
		}
	}
}
