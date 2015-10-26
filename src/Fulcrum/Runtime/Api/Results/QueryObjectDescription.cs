using System.Collections.Generic;
using Fulcrum.Common.JsonSchema;

namespace Fulcrum.Runtime.Api.Results
{
	public class QueryObjectDescription
	{
		public QueryObjectDescription(IList<JsonLink> links, IList<QueryDescription> queries)
		{
			Queries = queries;
			Links = links;
		}

		public IList<JsonLink> Links { get; private set; }

		public IList<QueryDescription> Queries { get; private set; }
	}
}
