using System;
using System.Web.Mvc;
using Fulcrum.Core;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.Api.Results.CommandPublication;

namespace Tests.ApiHarness.Controllers
{
	public class CommandPublisherController : BaseMvcController
	{
		private readonly ICommandPipeline _commandPipeline;

		public CommandPublisherController()
		{
		}

		public CommandPublisherController(ICommandPipeline commandPipeline)
		{
			_commandPipeline = commandPipeline;
		}

		[Route("commands/{inNamespace}/{name}/publish")]
		public JsonResult Publish(string inNamespace, string name,
			[ModelBinder(typeof(CommandModelBinder))] ICommand command)
		{
			if (ModelState.IsValid)
			{
				var record = _commandPipeline.Publish(command);

				if (record.Status == CommandPublicationStatus.Failed)
				{
					return Json(new PublicationFailureResult(record));
				}
				else
				{
					return Json(new PublicationImmediateResult(record));
				}
			}
			else
			{
				// return validation errors
			}

			throw new Exception("Whatever!");
		}
	}
}
