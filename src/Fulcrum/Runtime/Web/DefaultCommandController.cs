using System;
using System.Linq;
using System.Web.Mvc;
using Fulcrum.Common;
using Fulcrum.Core;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.Api.Results;
using Fulcrum.Runtime.Api.Results.CommandPublication;

namespace Fulcrum.Runtime.Web
{
	/// <summary>
	///   Provides an HTTP API for listing visible commands, publishing them,
	///   and viewing the command publication records associated with them.
	///   To use, create a concrete implementation of this abstract controller
	///   in your web project's Controllers directory, override the virtual methods,
	///   and define your own routes on them as attributes. If you prefer, you can
	///   use System.Web.Routing.
	/// </summary>
	public abstract class DefaultCommandController : BaseMvcController, ILoggingSource
	{
		private readonly ICommandLocator _commandLocator;

		private readonly ICommandPipeline _commandPipeline;

		protected DefaultCommandController(ICommandPipeline commandPipeline, ICommandLocator commandLocator)
		{
			_commandPipeline = commandPipeline;
			_commandLocator = commandLocator;
		}

		public virtual ActionResult Detail(string inNamespace, string name)
		{
			var commandType = _commandLocator.Find(name, inNamespace);

			if (commandType != null)
			{
				return Json(new CommandDescription(commandType, false));
			}

			return Json(new { error = "Unable to locate specified command." });
		}

		public virtual ActionResult Inquire(Guid publicationId)
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

		public virtual JsonResult ListAll()
		{
			var commands = _commandLocator.ListAllCommands();

			var descriptions = commands.Select(command => new CommandDescription(command, true));

			return JsonWithoutNulls(descriptions.ToList());
		}

		public virtual JsonResult PublicationGetWarning()
		{
			return Json(new { error = "HTTP POST only." });
		}

		public virtual JsonResult Publish(string inNamespace, string commandName,
			[ModelBinder(typeof(CommandModelBinder))] ICommand command)
		{
			return publish(command);
		}

		private JsonResult publish(ICommand command)
		{
			// This is not command-level validation, rather this is 
			// validation that all of the command's properties are intact.
			// Command handlers are responsible for their own validation
			// so "incomplete" commands may still be published in order
			// to support features such as synchronization of work in 
			// progress to multiple devices.
			if (ModelState.IsValid)
			{
				try
				{
					var record = _commandPipeline.Publish(command);

					if (record.Status == CommandPublicationStatus.Failed)
					{
						Response.StatusCode = 500;

						return Json(new CommandFailedResult(record));
					}

					return Json(new CommandCompleteOrPendingResult(record));
				}
				catch (Exception e)
				{
					this.LogFatal("Error publishing {0}.", e, command.GetType().Name);
				}
			}

			var states = ModelState.Values.Where(x => x.Errors.Count >= 1);

			var errorMessages = (from state in states
			                     from error in state.Errors
			                     select error.ErrorMessage);

			var errorList = errorMessages.ToList();

			Response.StatusCode = 500;

			this.LogWarn("Publication of invalid {0} command. {1}",
				command.GetType().Name, string.Join(",", errorList));

			return JsonWithoutNulls(new { errors = errorList });
		}
	}
}
