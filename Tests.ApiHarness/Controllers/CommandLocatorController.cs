using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Fulcrum.Runtime;
using Fulcrum.Runtime.Api.Resources;

namespace Tests.ApiHarness.Controllers
{
	public class CommandLocatorController : ApiController
	{
		private readonly ICommandLocator _commandLocator;

		public CommandLocatorController(ICommandLocator commandLocator)
		{
			_commandLocator = commandLocator;
		}

		[Route("commands/{inNamespace}/{name}")]
		[HttpGet]
		public CommandDescription Detail(string inNamespace, string name)
		{
			var commandType = _commandLocator.FindInNamespace(name, inNamespace);

			if (commandType != null)
			{
				return new CommandDescription(commandType);
			}

			throw new HttpResponseException(HttpStatusCode.NotFound);
		}

		[Route("commands")]
		[HttpGet]
		public IEnumerable<CommandDescription> ListAll()
		{
			var commands = _commandLocator.ListAllCommands();

			var descriptions = commands.Select(command => new CommandDescription(command));

			return descriptions.ToList();
		}
	}
}
