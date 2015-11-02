using Fulcrum.Common;
using Fulcrum.Core;
using FulcrumSeed.Components.UserAccounts.Commands;
using FulcrumSeed.Components.UserAccounts.Domain.Services;

namespace FulcrumSeed.Components.UserAccounts.CommandHandlers
{
	public class RegisterAccountHandler : ILoggingSource, ICommandHandler<RegisterAccount>
	{
		private readonly UserAccountService _userSvc;

		public RegisterAccountHandler(UserAccountService userSvc)
		{
			_userSvc = userSvc;
		}

		public void Handle(RegisterAccount command)
		{
			_userSvc.CreateAccount(command.Email, command.Password, command.Email);
		}
	}
}
