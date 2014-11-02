using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Runtime;
using Fulcrum.Runtime.Api.Results;

namespace Tests.ApiHarness.Controllers
{
	[RoutePrefix("queries")]
	public class QueryController : ApiController
	{
		private readonly IQueryLocator _queryLocator;

		public QueryController(IQueryLocator queryLocator)
		{
			_queryLocator = queryLocator;
		}

		[Route("")]
		[HttpGet]
		public IList<QueryDescription> ListAll()
		{
			var queryGroups = _queryLocator.ListAllQueryGroups();

			var result = (from @group in queryGroups
				let methods = _queryLocator.ListQueriesInQueryObject(@group)
				from method in methods
				select new QueryDescription(@group.Name, @group.Namespace, method, true, true)).ToList();

			return result;
		}

		[Route("{inNamespace}/{queryObjectName}/{query}")]
		[HttpGet]
		public QueryDescription QueryDetails(string inNamespace, string queryObjectName, string query)
		{
			var queryGroup = _queryLocator.FindInNamespace(queryObjectName, inNamespace);

			var queries = _queryLocator.ListQueriesInQueryObject(queryGroup);

			var queryDescription = queries.Where(q => q.Name == query).Select(q =>
				new QueryDescription(queryGroup.Name, queryGroup.Namespace, q, false, true)).SingleOrDefault();

			if (queryDescription != null)
			{
				queryDescription.Links.Add(new JsonLink("/queries/", "home"));
			}

			return queryDescription;
		}

		[Route("{inNamespace}/{queryObjectName}")]
		[HttpGet]
		public QueryObjectDescription  QueryObjectDetails(string inNamespace, string queryObjectName)
		{
			var queryGroup = _queryLocator.FindInNamespace(queryObjectName, inNamespace);

			var queries = _queryLocator.ListQueriesInQueryObject(queryGroup);

			var descriptions = queries.Select(query =>
				new QueryDescription(queryGroup.Name, queryGroup.Namespace, query, true, false)).ToList();

			var links = new List<JsonLink> { new JsonLink("/queries/", "home") };

			return new QueryObjectDescription(links, descriptions);
		}
	}
}
