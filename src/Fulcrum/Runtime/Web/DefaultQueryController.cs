using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Fulcrum.Common;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Core;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.Api.Results;

namespace Fulcrum.Runtime.Web
{
	/// <summary>
	///   Provides an HTTP API for listing visible queries as well as executing
	///   them and viewing their results.
	///   To use, create a concrete implementation of this abstract controller
	///   in your web project's Controllers directory, override the virtual methods,
	///   and define your own routes on them as attributes. If you prefer, you can
	///   use System.Web.Routing.
	/// </summary>
	public abstract class DefaultQueryController : BaseMvcController
	{
		private readonly IWindsorContainer _container;

		private readonly IQueryLocator _queryLocator;

		protected DefaultQueryController(IQueryLocator queryLocator, IWindsorContainer container)
		{
			_queryLocator = queryLocator;
			_container = container;
		}

		public virtual ActionResult ListAll()
		{
			var queryObjects = _queryLocator.ListAllQueryObjects();

			var result = (from queryObject in queryObjects
			              let methods = _queryLocator.ListQueriesInQueryObject(queryObject)
			              from method in methods
			              select new QueryDescription(queryObject.Name, queryObject.Namespace, method, true, true)).ToList();

			return Json(result);
		}

		public virtual ActionResult QueryDetails(string inNamespace, string queryObjectName, string query)
		{
			var queryObject = _queryLocator.Find(queryObjectName, inNamespace);

			var queries = _queryLocator.ListQueriesInQueryObject(queryObject);

			var queryDescription = queries.Where(q => q.Name == query).Select(q =>
			                                                                  new QueryDescription(queryObject.Name, queryObject.Namespace, q, false, true))
			                              .SingleOrDefault();

			if (queryDescription != null)
			{
				queryDescription.Links.Add(new JsonLink("/queries/", "home"));
			}

			return Json(queryDescription);
		}

		public virtual ActionResult QueryObjectDetails(string inNamespace, string queryObjectName)
		{
			var queryObject = _queryLocator.Find(queryObjectName, inNamespace);

			var queries = _queryLocator.ListQueriesInQueryObject(queryObject);

			var descriptions = queries.Select(query =>
			                                  new QueryDescription(queryObject.Name, queryObject.Namespace, query, true, false)).ToList();

			var links = new List<JsonLink> { new JsonLink("/queries/", "home") };

			return Json(new QueryObjectDescription(links, descriptions));
		}

		public virtual ActionResult Results(string inNamespace, string queryObjectName, string query)
		{
			_container.BeginScope();

			var queryObject = _queryLocator.Find(queryObjectName, inNamespace);

			if (queryObject == null)
			{
				var msg = string.Format("Unable to locate query {0}", queryObjectName);

				return Json(new { error = msg });
			}

			var queries = _queryLocator.ListQueriesInQueryObject(queryObject);

			var queryMethod = queries.SingleOrDefault(q => q.Name == query);

			var queryImplementation = _container.Resolve(queryObject);

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
						else if (parameter.ParameterType == typeof(DateTime))
						{
							try
							{
								parameterValues[paramIndex] = DateTime.Parse(parameterInRequest);
							}
							catch (Exception)
							{
								missingParams.Add(parameter.Name);
							}
						}
						else if (parameter.ParameterType == typeof(DateTime?))
						{
							try
							{
								if (string.IsNullOrWhiteSpace(parameterInRequest)
								    || parameterInRequest == "undefined" || parameterInRequest == "null")
								{
									parameterValues[paramIndex] = new DateTime?();
								}
								else
								{
									parameterValues[paramIndex] = DateTime.Parse(parameterInRequest);
								}
							}
							catch (Exception)
							{
								missingParams.Add(parameter.Name);
							}
						}
						else if (parameter.ParameterType == typeof(int?))
						{
							try
							{
								if (string.IsNullOrWhiteSpace(parameterInRequest)
								    || parameterInRequest == "undefined" || parameterInRequest == "null")
								{
									parameterValues[paramIndex] = new int?();
								}
								else
								{
									parameterValues[paramIndex] = Convert.ToInt32(parameterInRequest);
								}
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
						else if (parameter.ParameterType == typeof(Guid))
						{
							try
							{
								parameterValues[paramIndex] = Guid.Parse(parameterInRequest);
							}
							catch (Exception)
							{
								missingParams.Add(parameter.Name);
							}
						}
						else if (parameter.ParameterType == typeof(Guid?))
						{
							try
							{
								if (string.IsNullOrWhiteSpace(parameterInRequest)
								    || parameterInRequest == "undefined" || parameterInRequest == "null")
								{
									parameterValues[paramIndex] = new Guid?();
								}
								else
								{
									parameterValues[paramIndex] = Guid.Parse(parameterInRequest);
								}
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

				var totalResults = 0;
				var pageSize = 0;
				var totalPages = 0;
				var skip = 0;
				var currentPage = 0;

				var pagedList = results as IPagedList;

				if (pagedList != null)
				{
					pageSize = pagedList.PageSize;
					skip = pagedList.Skip;

					if (pagedList.Count > 0)
					{
						totalResults = pagedList.TotalResults;
						totalPages = Math.Max(1, (totalResults + pageSize - 1)) / pageSize;
						currentPage = pagedList.CurrentPage;
					}
				}

				// TODO: this would be more powerful if we could inject pagination links directly
				var wrappedResults = new
				{
					links = new List<JsonLink>
					{
						new JsonLink("/api/queries/", "home"),
						new JsonLink(string.Format("/api/queries/{0}/{1}/{2}", inNamespace, queryObjectName, query), "details")
					},
					results, totalResults, pageSize, totalPages, skip, currentPage
				};

				_container.Release(queryImplementation);

				return Json(wrappedResults);
			}

			return Json(string.Format("Unable to locate implementation for query {0}", query));
		}

		public virtual ActionResult Validate(string inNamespace, string queryObjectName,
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

		public virtual ActionResult ValidateGetWarning(string inNamespace, string queryObjectName)
		{
			return Json(new { error = "HTTP POST only." });
		}
	}
}
