using System;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Fulcrum.Runtime.CommandPipeline;
using FulcrumSeed.WebApi.App_Start;

namespace FulcrumSeed.WebApi
{
	public class Global : HttpApplication
	{
		private void Application_Start(object sender, EventArgs e)
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);


			Database.SetInitializer(new MigrateDatabaseToLatestVersion<CommandPipelineDbContext, Fulcrum.Migrations.Configuration>());

		}
	}
}
