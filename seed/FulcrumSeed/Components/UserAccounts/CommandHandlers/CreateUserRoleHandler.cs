using System;
using System.Collections.Generic;
using Fulcrum.Common;
using Fulcrum.Core;
using Fulcrum.Runtime.CommandPipeline;
using FulcrumSeed.Components.UserAccounts.Commands;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Components.UserAccounts.Queries;

namespace FulcrumSeed.Components.UserAccounts.CommandHandlers
{
	public class CreateUserRoleHandler : ILoggingSource, ICommandHandler<CreateUserRole>
	{
		private readonly UserGroupRepository _repo;

		private readonly CommandPublicationRegistry _registry;

		private readonly ICommandPipeline _pipeline;

		public CreateUserRoleHandler(UserGroupRepository repo, CommandPublicationRegistry registry)
		{
			_repo = repo;
			_registry = registry;
		}

		public void Handle(CreateUserRole command)
		{
			if (_repo.GetByName("", command.Name) != null)
			{
				throw new Exception("A group by this name already exists.");
			}

			var newRole = new UserRole(command.Name)
			{
				Description = command.Description,
			};

			_repo.Add(newRole);

			var queryName = QueryReferencer.GetName<UserRoleQueries>(q => q.FindById(newRole.ID));

			_registry.AssociateQueryReference(command.PublicationRecordId, new ParameterizedQueryReference
			{
				QueryName = queryName,
				Parameters = new List<QueryReferenceParameter>
				{
					new QueryReferenceParameter
					{
						Name = "id",
						Value = newRole.ID.ToString(),
						Id = Guid.NewGuid()
					}
				}
			});
		}
	}

	public class EditUserRoleHandler : ILoggingSource, ICommandHandler<EditUserRole>
	{
		private readonly UserGroupRepository _repo;

		public EditUserRoleHandler(UserGroupRepository repo)
		{
			_repo = repo;
		}

		public void Handle(EditUserRole command)
		{
			var userRole = _repo.GetByID(command.RoleId);

			if (userRole == null)
			{
				throw new Exception("No such role exists..");
			}

			userRole.Description = command.Description;

			_repo.Update(userRole);
		}
	}
}
