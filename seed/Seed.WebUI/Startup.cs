using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter.Unofficial;
using Fulcrum.Common;
using Fulcrum.Common.Web;
using Fulcrum.Runtime.TiltedGlobe.Runtime;
using log4net.Config;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Owin;
using SeedComponents;

namespace Seed.WebUI
{
	public partial class Startup : ILoggingSource
	{
		private IWindsorContainer _container;

		public void Configuration(IAppBuilder app)
		{
			XmlConfigurator.Configure();

			var httpConfig = new HttpConfiguration();

			_container = new WindsorContainer();

			CommonAppSetup.ConfigureContainer<UserSystemDb, UserSystemSettings>(_container);
			CommonAppSetup.ConfigureCommandsAndHandlers(_container);
			CommonAppSetup.ConfigureQueries(_container);

			configureWebApi(httpConfig);

			app.UseCors(CorsOptions.AllowAll);

			app.UseWebApi(httpConfig);

			configureMvc();
		}

		private void configureMvc()
		{
			_container.Register(Classes.FromAssemblyInThisApplication()
			                           .BasedOn<ApiController>()
			                           .LifestyleTransient());

			// set up mvc di
			_container.Register(Classes.FromThisAssembly()
			                           .BasedOn<IController>()
			                           .LifestyleTransient());

			var controllerFactory = new WindsorControllerFactory(_container.Kernel);

			ControllerBuilder.Current.SetControllerFactory(controllerFactory);

			DependencyResolver.SetResolver(new WindsorServiceLocator(_container));
		}

		private void configureWebApi(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				"DefaultApi",
				"api/{controller}/{id}",
				new { id = RouteParameter.Optional });

			var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();

			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

			GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(_container.Kernel);

			config.DependencyResolver = new WindsorDependencyResolver(_container.Kernel);

			config.Services.Replace(typeof(IHttpControllerActivator), new WindsorControllerActivator(_container));
		}
	}
}
