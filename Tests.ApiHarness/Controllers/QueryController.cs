using System.Collections.Generic;
using System.Web.Http;
using Fulcrum.Runtime.Api.Results;
using Newtonsoft.Json.Schema;

namespace Tests.ApiHarness.Controllers
{
	[RoutePrefix("queries")]
	public class QueryController : ApiController
	{
		[Route("")]
		[HttpGet]
		public IList<QueryDescription> ListAll()
		{
			return new List<QueryDescription>();
		}

		[Route("{groupName}")]
		[HttpGet]
		public QueryDescription Details(string groupName)
		{
			return new QueryDescription("", "", new JsonSchema());
		}
	}
}
