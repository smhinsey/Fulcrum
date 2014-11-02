using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Fulcrum.Core;
using Fulcrum.Runtime;
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


	}
}
