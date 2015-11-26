using System;
using Fulcrum.Common;
using Fulcrum.Core;
using FulcrumSeed.Components.UserAccounts.Commands;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;

namespace FulcrumSeed.Components.UserAccounts.CommandHandlers
{
	public class CreateUserRoleHandler : ILoggingSource, ICommandHandler<CreateUserRole>
	{
		private readonly UserGroupRepository _repo;

		public CreateUserRoleHandler(UserGroupRepository repo)
		{
			_repo = repo;
		}

		public void Handle(CreateUserRole command)
		{
			if (_repo.GetByName("", command.Name) != null)
			{
				throw new Exception("A group by this name already exists.");
			}

			_repo.Add(new UserRole(command.Name)
			{
				Description = command.Description,
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
