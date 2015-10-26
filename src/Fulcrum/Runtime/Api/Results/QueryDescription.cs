using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results
{
	public class QueryDescription : QueryDescriptor
	{
		public QueryDescription(string queryObject, string @namespace, MethodInfo methodInfo, bool includeDetailsLink, bool includeQueryObjectLink)
			: base(queryObject, @namespace, methodInfo)
		{
			Parameters = new Dictionary<string, string>();

			foreach (var parameter in methodInfo.GetParameters()
				.Where(parameter => !Parameters.ContainsKey(parameter.Name)))
			{
				Parameters.Add(parameter.Name, parameter.ParameterType.Name);
			}

			Links = new List<JsonLink>();

			if (includeDetailsLink)
			{
				Links.Add(new JsonLink(string.Format("/api/queries/{0}/{1}/{2}", Namespace, QueryObject, Query), "details"));
			}

			if (includeQueryObjectLink)
			{
				Links.Add(new JsonLink(string.Format("/api/queries/{0}/{1}", Namespace, QueryObject), "queryObject"));
			}

			var queryString = string.Empty;

			var paramCounter = 0;

			Parameters.Keys.ForEach(k =>
			{
				queryString += k;
				queryString += string.Format("={{{0}}}", paramCounter + 1);

				if (paramCounter < Parameters.Keys.Count - 1)
				{
					queryString += "&";
				}

				paramCounter++;
			});

			var isValidationQuery = false;

			var queryObjectType = methodInfo.DeclaringType;

			if (queryObjectType.GetInterfaces().Any(i => typeof(ICommandValidationQuery).IsAssignableFrom(i)))
			{
				isValidationQuery = true;
			}

			if (isValidationQuery)
			{
				Links.Add(new JsonLink(string.Format("/api/queries/{0}/{1}/{2}/validate", 
					Namespace, QueryObject, Query), "results"));
			}
			else
			{
				Links.Add(new JsonLink(string.Format("/api/queries/{0}/{1}/{2}/results?{3}", 
					Namespace, QueryObject, Query, queryString), "results"));
			}
		}

		public List<JsonLink> Links { get; private set; }

		public IDictionary<string, string> Parameters { get; private set; }
	}
}
