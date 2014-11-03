using System;
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
			var queryObject = _queryLocator.FindInNamespace(queryObjectName, inNamespace);

			var queries = _queryLocator.ListQueriesInQueryObject(queryObject);

			var descriptions = queries.Select(query =>
				new QueryDescription(queryObject.Name, queryObject.Namespace, query, true, false)).ToList();

			var links = new List<JsonLink> { new JsonLink("/queries/", "home") };

			return Json(new QueryObjectDescription(links, descriptions));
		}

		[Route("{inNamespace}/{queryObjectName}/{query}/results")]
		[HttpGet]
		public ActionResult Results(string inNamespace, string queryObjectName, string query)
		{
			var queryObject = _queryLocator.FindInNamespace(queryObjectName, inNamespace);

			var queries = _queryLocator.ListQueriesInQueryObject(queryObject);

			var queryMethod = queries.SingleOrDefault(q => q.Name == query);

			var queryImplementation = DependencyResolver.Current.GetService(queryObject);

			if (queryMethod != null)
			{
				var parameters = queryMethod.GetParameters();

				var parameterValues = new object[parameters.Length];

				var missingParams = new List<string>();

				var paramIndex = 0;

				foreach (var parameter in parameters)
				{
					var parameterInRequest = Request.Params[parameter.Name];

					if (parameterInRequest == null)
					{
						missingParams.Add(parameter.Name);
					}
					else
					{
						if (parameter.ParameterType == typeof(string))
						{
							parameterValues[paramIndex] = parameterInRequest;
						}
						else if (parameter.ParameterType == typeof(int))
						{
							parameterValues[paramIndex] = Convert.ToInt32(parameterInRequest);
						}
						else if (parameter.ParameterType == typeof(bool))
						{
							parameterValues[paramIndex] = Convert.ToBoolean(parameterInRequest);
						}
						else if (parameter.ParameterType == typeof(decimal))
						{
							parameterValues[paramIndex] = Convert.ToDecimal(parameterInRequest);
						}
						else if (parameter.ParameterType == typeof(double))
						{
							parameterValues[paramIndex] = Convert.ToDouble(parameterInRequest);
						}
					}

					paramIndex++;
				}

				return Json(queryMethod.Invoke(queryImplementation, parameterValues));
			}

			return Json(string.Format("Unable to locate implementation for query {0}", query));
		}
	}
}
