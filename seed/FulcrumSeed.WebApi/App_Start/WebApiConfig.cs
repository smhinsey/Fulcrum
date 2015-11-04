using System.Web.Http;

namespace FulcrumSeed.WebApi.App_Start
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();

			// use attribute or explicit routes only as /api is bound to Fulcrum's api
			//config.Routes.MapHttpRoute(
			//	name: "DefaultApi",
			//	routeTemplate: "api/{controller}/{id}",
			//	defaults: new { id = RouteParameter.Optional }
			//	);
		}
	}
}
