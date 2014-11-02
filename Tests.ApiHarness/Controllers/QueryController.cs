using System.Collections.Generic;
using System.Web.Http;
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

		[Route("{groupName}")]
		[HttpGet]
		public QueryDescription Details(string groupName)
		{
			return null;
		}

		[Route("")]
		[HttpGet]
		public IList<QueryDescription> ListAll()
		{
			return new List<QueryDescription>();
		}
	}
}
