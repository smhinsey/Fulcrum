using System;
using Fulcrum.Common;
using Fulcrum.Core;
using FulcrumSeed.Components.UserAccounts.Commands;
using FulcrumSeed.Components.UserAccounts.Domain.Services;

namespace FulcrumSeed.Components.UserAccounts.CommandHandlers
{
	public class EditAccountHandler : ILoggingSource, ICommandHandler<EditAccount>
	{
		private readonly AppUserService _appUserSvc;

		public EditAccountHandler(AppUserService appUserSvc)
		{
			_appUserSvc = appUserSvc;
		}

		public void Handle(EditAccount command)
		{
			var account = _appUserSvc.GetByID(command.Id);

			if (account == null)
			{
				var message = string.Format("Unable to locate user {0}", command.Id);

				this.LogWarn(message);

				throw new Exception(message);
			}

			account.FirstName = command.FirstName;
			account.LastName = command.LastName;

			_appUserSvc.Update(account);

			if (command.Email != account.Email)
			{
				_appUserSvc.ChangeEmailRequest(account.ID, command.Email);
			}
		}
	}
}
