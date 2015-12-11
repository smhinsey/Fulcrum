using System.Collections.Generic;

namespace Fulcrum.Runtime.Api.Results.CommandPublication
{
	public class QueryReferenceResult
	{
		public string QueryName { get; set; }

		public List<QueryReferenceParameterResult> Parameters { get; set; }
	}
}