using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Examples.UserProfileBC.Commands;
using Fulcrum.Common.Web;
using Fulcrum.Runtime;
using Tests.Unit.Commands.Validation;

namespace Tests.ApiHarness
{
	public class WebApiApplication : HttpApplication
	{
		private CommandLocator _commandLocator;

		private IWindsorContainer _container;

		protected void Application_Start()
		{
			configureCommandLocations();
			configureContainer();

			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		private void configureCommandLocations()
		{
			_commandLocator = new CommandLocator(typeof(RegisterUser).Assembly, typeof(RegisterUser).Namespace);

			// TODO: Create generalized config system
			_commandLocator.AddCommandSource(typeof(SchemaTestingCommand).Assembly, typeof(SchemaTestingCommand).Namespace);
		}

		private void configureContainer()
		{
			_container = new WindsorContainer();

			_container.Register(Classes.FromAssemblyInThisApplication()
				.BasedOn<ApiController>()
				.LifestylePerWebRequest());

			_container.Register(
				Component.For<ICommandLocator>()
					.UsingFactoryMethod(() => _commandLocator)
				);

			GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(_container.Kernel);

			GlobalConfiguration.Configuration.Services.Replace(
				typeof(IHttpControllerActivator),
				new WindsorControllerActivator(_container));
		}
	}
}
