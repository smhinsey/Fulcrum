using System;
using System.Web.Mvc;
using Fulcrum.Core;
using Fulcrum.Runtime;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.Web;

namespace Tests.ApiHarness.Controllers
{
	[RoutePrefix("commands")]
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
		public override ActionResult Inquire(Guid publicationId)
		{
			return base.Inquire(publicationId);
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
		//[Authorize]
		public override JsonResult Publish(string inNamespace, string commandName,
			[ModelBinder(typeof(CommandModelBinder))] ICommand command)
		{
			return base.Publish(inNamespace, commandName, command);
		}
	}
}
