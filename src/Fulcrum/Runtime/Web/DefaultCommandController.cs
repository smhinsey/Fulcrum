using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Mvc;
using Fulcrum.Common;
using Fulcrum.Core;
using Fulcrum.Core.Security;
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
				// TODO: filter command by claims
				
				return JsonWithNulls(new CommandDescription(commandType, false));
			}

			return Json(new { error = "Unable to locate specified command." });
		}

		public virtual ActionResult RegistryDetails(Guid publicationId)
		{
			var record = _commandPipeline.GetRecordById(publicationId);

			if (record != null)
			{
				return JsonWithNulls(new DetailedPublicationRecordResult(record));
			}

			return JsonWithoutNulls(new
			{
				error = string.Format("Record {0} not found in command registry.", publicationId)
			});
		}

		public virtual JsonResult ListRegistry()
		{
			var records = _commandPipeline.GetRegistryPage(100,0);

			// TODO: filter access based on the claims of the command associated with the record

			var descriptions = records.Select(record => new DetailedPublicationRecordResult(record));

			return JsonWithoutNulls(descriptions.ToList());
		}

		public virtual JsonResult ListAll()
		{
			var commands = _commandLocator.ListAllCommands();

			// TODO: filter commands by claims

			var descriptions = commands.Select(command => new CommandDescription(command, true));

			return JsonWithoutNulls(descriptions.ToList());
		}

		public virtual JsonResult PublicationGetWarning()
		{
			return Json(new { error = "HTTP POST only." });
		}

		public virtual ActionResult Publish(string inNamespace, string commandName,
			[ModelBinder(typeof(CommandModelBinder))] ICommand command)
		{
			return publish(command);
		}

		private ActionResult publish(ICommand command)
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
					// TODO: factor this out
					var customAttributes = command.GetType().GetCustomAttributes(typeof(RequiresClaimAttribute), false);

					var claimsDemandsAttrs = new List<Attribute>(customAttributes.Cast<Attribute>());

					if (claimsDemandsAttrs.Any())
					{
						var safeToProceed = false;

						var claimsPrincipal = User as ClaimsPrincipal;

						if (claimsPrincipal != null)
						{
							var userClaims = claimsPrincipal.Claims;

							if (userClaims != null && userClaims.Count() >= claimsDemandsAttrs.Count())
							{
								foreach (var claimAttr in claimsDemandsAttrs)
								{
									var demandedClaim = claimAttr as RequiresClaimAttribute;

									if (demandedClaim == null)
									{
										this.LogDebug("claimAttr is null. This is unexpected.");
									}
									else
									{
										var matchedClaim = userClaims.SingleOrDefault(c => c.Type == demandedClaim.Type &&
																																			 c.Value == demandedClaim.Value);

										if (matchedClaim != null)
										{
											safeToProceed = true;
										}
										else
										{
											this.LogWarn("User {0} doesn't possess claim of type {1} with value {2} required for command {3}.",
												User.Identity.Name, demandedClaim.Type, demandedClaim.Value, command.GetType().Name);
										}
									}
								}
							}
							else
							{
								// fail
							}
						}
						else
						{
							this.LogWarn("This command requires that the Authorize attribute be present on the command controller's Publish action.");
						}

						if (!safeToProceed)
						{
							return new HttpUnauthorizedResult(string.Format("User {0} lacks the claims required for command {1}. See logs for more.",
								User.Identity.Name, command.GetType().Name));
						}
					}

					var record = _commandPipeline.Publish(command);

					if (record.Status == CommandPublicationStatus.Failed)
					{
						Response.StatusCode = 500;

						return JsonWithNulls(new CommandFailedResult(record));
					}

					return JsonWithNulls(new CommandCompleteOrPendingResult(record));
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
