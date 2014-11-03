using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Core;
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
			var queryObjects = _queryLocator.ListAllQueryObjects();

			var result = (from queryObject in queryObjects
				let methods = _queryLocator.ListQueriesInQueryObject(queryObject)
				from method in methods
				select new QueryDescription(queryObject.Name, queryObject.Namespace, method, true, true)).ToList();

			return Json(result);
		}

		[Route("{inNamespace}/{queryObjectName}/{query}")]
		[HttpGet]
		public ActionResult QueryDetails(string inNamespace, string queryObjectName, string query)
		{
			var queryObject = _queryLocator.FindInNamespace(queryObjectName, inNamespace);

			var queries = _queryLocator.ListQueriesInQueryObject(queryObject);

			var queryDescription = queries.Where(q => q.Name == query).Select(q =>
				new QueryDescription(queryObject.Name, queryObject.Namespace, q, false, true)).SingleOrDefault();

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

				// TODO: this should be factored out
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
							try
							{
								parameterValues[paramIndex] = Convert.ToInt32(parameterInRequest);
							}
							catch (Exception)
							{
								missingParams.Add(parameter.Name);
							}
						}
						else if (parameter.ParameterType == typeof(bool))
						{
							try
							{
								parameterValues[paramIndex] = Convert.ToBoolean(parameterInRequest);
							}
							catch (Exception)
							{
								missingParams.Add(parameter.Name);
							}
						}
						else if (parameter.ParameterType == typeof(decimal))
						{
							try
							{
								parameterValues[paramIndex] = Convert.ToDecimal(parameterInRequest);
							}
							catch (Exception)
							{
								missingParams.Add(parameter.Name);
							}
						}
						else if (parameter.ParameterType == typeof(double))
						{
							try
							{
								parameterValues[paramIndex] = Convert.ToDouble(parameterInRequest);
							}
							catch (Exception)
							{
								missingParams.Add(parameter.Name);
							}
						}
						else
						{
							return Json(new
							{
								error = string.Format("Query parameter {0}'s type, {1}, is not supported and this query cannot be executed.",
									parameter.Name, parameter.ParameterType.Name)
							});
						}
					}

					paramIndex++;
				}

				if (missingParams.Count > 0)
				{
					return Json(new
					{
						error = "Unable to execute query due to missing or invalid parameter(s).", missingOrInvalidParameters = missingParams
					});
				}

				var results = queryMethod.Invoke(queryImplementation, parameterValues);

				// TODO: we need a scheme for identifying paginated queries and supporting them here
				var wrappedResults = new
				{
					links = new List<JsonLink>
					{
						new JsonLink("/queries/", "home"),
						new JsonLink(string.Format("/queries/{0}/{1}/{2}", inNamespace, queryObjectName, query), "details")
					},
					results
				};

				return Json(wrappedResults);
			}

			return Json(string.Format("Unable to locate implementation for query {0}", query));
		}

		[Route("{inNamespace}/{queryObjectName}/validate")]
		[HttpPost]
		public ActionResult Validate(string inNamespace, string queryObjectName,
			[ModelBinder(typeof(CommandModelBinder))] ICommand command)
		{
			// TODO: how do we tell the model binder what type of command it is?

			// See comments in CommandController.Publish for more on what is meant by IsValid
			if (ModelState.IsValid)
			{
				// TODO: implement validation query execution
				return Json("Command validation queries not yet implemented.");
			}

			var states = ModelState.Values.Where(x => x.Errors.Count >= 1);

			var errorMessages = (from state in states
				from error in state.Errors
				select error.ErrorMessage);

			return JsonWithoutNulls(new { errors = errorMessages.ToList() });
		}

		[Route("{inNamespace}/{queryObjectName}/validate")]
		[HttpGet]
		public ActionResult ValidateGetWarning(string inNamespace, string queryObjectName)
		{
			return Json(new { error = "HTTP POST only." });
		}
	}
}
