using System.Web.Mvc;
using Castle.Windsor;
using Fulcrum.Core;
using Fulcrum.Runtime;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.Web;

namespace FulcrumSeed.WebUI.Controllers
{
	[RoutePrefix("api/queries")]
	public class QueryController : DefaultQueryController
	{
		public QueryController(IQueryLocator queryLocator, IWindsorContainer container)
			: base(queryLocator, container)
		{
		}

		[Route("")]
		[HttpGet]
		public override ActionResult ListAll()
		{
			return base.ListAll();
		}

		[Route("{inNamespace}/{queryObjectName}/{query}")]
		[HttpGet]
		public override ActionResult QueryDetails(string inNamespace, string queryObjectName, string query)
		{
			return base.QueryDetails(inNamespace, queryObjectName, query);
		}

		[Route("{inNamespace}/{queryObjectName}")]
		[HttpGet]
		public override ActionResult QueryObjectDetails(string inNamespace, string queryObjectName)
		{
			return base.QueryObjectDetails(inNamespace, queryObjectName);
		}

		[Route("{inNamespace}/{queryObjectName}/{query}/results")]
		//[Authorize]
		[HttpGet]
		public override ActionResult Results(string inNamespace, string queryObjectName, string query)
		{
			return base.Results(inNamespace, queryObjectName, query);
		}

		[Route("{inNamespace}/{queryObjectName}/validate")]
		[HttpPost]
		public override ActionResult Validate(string inNamespace, string queryObjectName,
			[ModelBinder(typeof(CommandModelBinder))] ICommand command)
		{
			return base.Validate(inNamespace, queryObjectName, command);
		}

		[Route("{inNamespace}/{queryObjectName}/validate")]
		[HttpGet]
		public override ActionResult ValidateGetWarning(string inNamespace, string queryObjectName)
		{
			return base.ValidateGetWarning(inNamespace, queryObjectName);
		}
	}
}