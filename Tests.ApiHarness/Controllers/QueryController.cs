using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Runtime;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.Api.Results;

namespace Tests.ApiHarness.Controllers
{
	[RoutePrefix("queries")]
	public class QueryController : BaseMvcController
	{
		private readonly IQueryLocator _queryLocator;

		public QueryController(IQueryLocator queryLocator)
		{
			_queryLocator = queryLocator;
		}

		[Route("")]
		[HttpGet]
		public ActionResult ListAll()
		{
			var queryGroups = _queryLocator.ListAllQueryGroups();

			var result = (from @group in queryGroups
				let methods = _queryLocator.ListQueriesInQueryObject(@group)
				from method in methods
				select new QueryDescription(@group.Name, @group.Namespace, method, true, true)).ToList();

			return Json(result);
		}

		[Route("{inNamespace}/{queryObjectName}/{query}")]
		[HttpGet]
		public ActionResult QueryDetails(string inNamespace, string queryObjectName, string query)
		{
			var queryGroup = _queryLocator.FindInNamespace(queryObjectName, inNamespace);

			var queries = _queryLocator.ListQueriesInQueryObject(queryGroup);

			var queryDescription = queries.Where(q => q.Name == query).Select(q =>
				new QueryDescription(queryGroup.Name, queryGroup.Namespace, q, false, true)).SingleOrDefault();

			if (queryDescription != null)
			{
				queryDescription.Links.Add(new JsonLink("/queries/", "home"));
			}

			return Json(queryDescription);
		}

		[Route("{inNamespace}/{queryObjectName}")]
		[HttpGet]
		public ActionResult QueryObjectDetails(string inNamespace, string queryObjectName)
		{
			var queryGroup = _queryLocator.FindInNamespace(queryObjectName, inNamespace);

			var queries = _queryLocator.ListQueriesInQueryObject(queryGroup);

			var descriptions = queries.Select(query =>
				new QueryDescription(queryGroup.Name, queryGroup.Namespace, query, true, false)).ToList();

			var links = new List<JsonLink> { new JsonLink("/queries/", "home") };

			return Json(new QueryObjectDescription(links, descriptions));
		}

		[Route("{inNamespace}/{queryObjectName}/{query}/results")]
		[HttpGet]
		public ActionResult Results()
		{
			return Json("Run query");
		}

	}
}
