using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Fulcrum.Core;
using Fulcrum.Runtime;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.Api.Results;

namespace Tests.ApiHarness.Controllers
{
	[RoutePrefix("commands")]
	public class CommandController : ApiController
	{
		private readonly ICommandLocator _commandLocator;

		private readonly ICommandPipeline _commandPipeline;

		public CommandController(ICommandLocator commandLocator, ICommandPipeline commandPipeline)
		{
			_commandLocator = commandLocator;
			_commandPipeline = commandPipeline;
		}

		[Route("{inNamespace}/{name}")]
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

		[Route("")]
		[HttpGet]
		public IEnumerable<CommandDescription> ListAll()
		{
			var commands = _commandLocator.ListAllCommands();

			var descriptions = commands.Select(command => new CommandDescription(command));

			return descriptions.ToList();
		}
	}
}
