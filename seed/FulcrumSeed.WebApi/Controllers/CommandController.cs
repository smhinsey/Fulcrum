using System;
using System.Web.Mvc;
using Fulcrum.Core;
using Fulcrum.Runtime;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.Web;

namespace FulcrumSeed.WebApi.Controllers
{
	[RoutePrefix("commands")]
	[Authorize]
	public class CommandController : DefaultCommandController
	{
		public CommandController(ICommandPipeline commandPipeline, ICommandLocator commandLocator)
			: base(commandPipeline, commandLocator)
		{
		}

		[Route("{inNamespace}/{name}")]
		[HttpGet]
		public override ActionResult Detail(string inNamespace, string name)
		{
			return base.Detail(inNamespace, name);
		}

		[Route("publication-registry/{publicationId}")]
		[HttpGet]
		public override ActionResult RegistryDetails(Guid publicationId)
		{
			return base.RegistryDetails(publicationId);
		}

		[Route("publication-registry")]
		[HttpGet]
		public override JsonResult ListRegistry()
		{
			return base.ListRegistry();
		}

		[Route("")]
		[HttpGet]
		public override JsonResult ListAll()
		{
			return base.ListAll();
		}

		[Route("{inNamespace}/{name}/publish")]
		[HttpGet]
		public override JsonResult PublicationGetWarning()
		{
			return base.PublicationGetWarning();
		}

		[Route("{inNamespace}/{commandName}/publish")]
		[HttpPost]
		public override ActionResult Publish(string inNamespace, string commandName,
			[ModelBinder(typeof(CommandModelBinder))] ICommand command)
		{
			return base.Publish(inNamespace, commandName, command);
		}
	}
}
