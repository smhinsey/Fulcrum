using System.Collections.Generic;
using System.Web.Http;
using Fulcrum.Runtime;
using Fulcrum.Runtime.Api.Results;
using Newtonsoft.Json.Schema;

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


			return new List<QueryDescription>();
		}

		[Route("{groupName}")]
		[HttpGet]
		public QueryDescription Details(string groupName)
		{
			
		}
	}
}
