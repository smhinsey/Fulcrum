using System;
using System.Linq;
using System.Web.Mvc;
using Fulcrum.Core;
using Fulcrum.Runtime;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.Api.Results;
using Fulcrum.Runtime.Api.Results.CommandPublication;

namespace Tests.ApiHarness.Controllers
{
	[RoutePrefix("commands")]
	public class CommandController : BaseMvcController
	{
		private readonly ICommandLocator _commandLocator;

		private readonly ICommandPipeline _commandPipeline;

		public CommandController(ICommandPipeline commandPipeline, ICommandLocator commandLocator)
		{
			_commandPipeline = commandPipeline;
			_commandLocator = commandLocator;
		}

		[Route("{inNamespace}/{name}")]
		[HttpGet]
		public ActionResult Detail(string inNamespace, string name)
		{
			var commandType = _commandLocator.FindInNamespace(name, inNamespace);

			if (commandType != null)
			{
				return Json(new CommandDescription(commandType, false));
			}

			return Json(new { error = "Unable to locate specified command." });
		}

		[Route("publication-registry/{publicationId}")]
		[HttpGet]
		public ActionResult Inquire(Guid publicationId)
		{
			var record = _commandPipeline.Inquire(publicationId);

			if (record != null)
			{
				return Json(new DetailedPublicationRecordResult(record));
			}

			return JsonWithoutNulls(new
			{
				error = string.Format("Record {0} not found in command registry.", publicationId)
			});
		}

		[Route("")]
		[HttpGet]
		public JsonResult ListAll()
		{
			var commands = _commandLocator.ListAllCommands();

			var descriptions = commands.Select(command => new CommandDescription(command, true));

			return JsonWithoutNulls(descriptions.ToList());
		}

		[Route("{inNamespace}/{name}/publish")]
		[HttpGet]
		public JsonResult PublicationGetWarning()
		{
			return Json(new { error = "HTTP POST only." });
		}

		[Route("{inNamespace}/{name}/publish")]
		[HttpPost]
		public JsonResult Publish(string inNamespace, string name,
			[ModelBinder(typeof(CommandModelBinder))] ICommand command)
		{
			// This is not command-level validation, rather this is 
			// validation that all of the command's properties are intact.
			// Command handlers are responsible for their own validation
			// so "incomplete" commands may still be published in order
			// to support features such as synchronization of work in 
			// progress to multiple devices.
			if (ModelState.IsValid)
			{
				var record = _commandPipeline.Publish(command);

				if (record.Status == CommandPublicationStatus.Failed)
				{
					return Json(new CommandFailedResult(record));
				}

				return Json(new CommandCompleteOrPendingResult(record));
			}

			var states = ModelState.Values.Where(x => x.Errors.Count >= 1);

			var errorMessages = (from state in states
				from error in state.Errors
				select error.ErrorMessage);

			return JsonWithoutNulls(new { errors = errorMessages.ToList() });
		}
	}
}
