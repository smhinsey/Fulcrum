using Fulcrum.Common;
using Fulcrum.Core;
using FulcrumSeed.Components.UserAccounts.Commands;
using FulcrumSeed.Components.UserAccounts.Domain.Services;

namespace FulcrumSeed.Components.UserAccounts.CommandHandlers
{
	public class RegisterAccountHandler : ILoggingSource, ICommandHandler<RegisterAccount>
	{
		private readonly AppUserService _appUserSvc;

		public RegisterAccountHandler(AppUserService appUserSvc)
		{
			_appUserSvc = appUserSvc;
		}

		public void Handle(RegisterAccount command)
		{
			var newAccount = _appUserSvc.CreateAccount(command.Email, command.Password, command.Email);

			this.LogInfo("Created account {0} for {1} {2}, {3}.",
				newAccount.ID, newAccount.FirstName, newAccount.LastName, newAccount.Email);
		}
	}
}
