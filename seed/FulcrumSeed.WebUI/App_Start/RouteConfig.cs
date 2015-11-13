using System.Web.Mvc;
using System.Web.Routing;

namespace FulcrumSeed.WebUI
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("favicon.ico");

			routes.MapMvcAttributeRoutes();

			routes.MapRoute(
				name: "Default",
				url: "",
				defaults: new { controller = "Shell", action = "Shell" }
				);
		}
	}
}
