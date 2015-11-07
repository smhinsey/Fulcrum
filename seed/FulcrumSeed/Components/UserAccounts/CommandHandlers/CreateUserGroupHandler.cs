using System;
using Fulcrum.Common;
using Fulcrum.Core;
using FulcrumSeed.Components.UserAccounts.Commands;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;

namespace FulcrumSeed.Components.UserAccounts.CommandHandlers
{
	public class CreateUserGroupHandler : ILoggingSource, ICommandHandler<CreateUserClaimGroup>
	{
		private readonly UserGroupRepository _repo;

		public CreateUserGroupHandler(UserGroupRepository repo)
		{
			_repo = repo;
		}

		public void Handle(CreateUserClaimGroup command)
		{
			if (_repo.GetByName("", command.Name) != null)
			{
				throw new Exception("A group by this name already exists.");
			}

			_repo.Add(new UserClaimGroup(command.Name)
			{
				Description = command.Description,
			
			});
		}
	}
}
