using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace FulcrumSeed.WebUI.Controllers
{
	[Route("api/user/claims")]
	[Authorize]
	public class TestController : ApiController
	{
		public IHttpActionResult Get()
		{
			var cp = User as ClaimsPrincipal;
			return Ok(cp.Claims.Select(x => new { x.Type, x.Value }));
		}
	}
}
