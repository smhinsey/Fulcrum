using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Examples.UserProfileComponent.Public.Commands;
using Fulcrum.Common.Web;
using Fulcrum.Core;
using Fulcrum.Runtime;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.CommandPipeline;
using Tests.Unit.Commands.Pipeline;
using Tests.Unit.Commands.Validation;
using Tests.Unit.Queries;
using Tests.Unit.Queries.Location.Additional;

namespace Tests.ApiHarness
{
	public class WebApiApplication : HttpApplication
	{
		private IWindsorContainer _container;

		protected void Application_Start()
		{
			configureContainer();
			configureQueryLocations();
			configureCommandLocations();
			configureCommandPipeline();

			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		private void configureCommandLocations()
		{
			var commandLocator = new CommandLocator(typeof(RegisterUser).Assembly, typeof(RegisterUser).Namespace);

			// TODO: Create generalized config system
			commandLocator.AddCommandSource(typeof(SchemaTestingCommand).Assembly, typeof(SchemaTestingCommand).Namespace);
			commandLocator.AddCommandSource(typeof(PingPipelineCommand).Assembly, typeof(PingPipelineCommand).Namespace);

			_container.Register(
				Component.For<ICommandLocator>()
					.UsingFactoryMethod(() => commandLocator)
				);
		}

		private void configureCommandPipeline()
		{
			ModelBinders.Binders.Add(typeof(ICommand), new CommandModelBinder());

			var handlers = PipelineInstaller.InstallHandlers(_container, typeof(PingPipelineCommand).Assembly);

			var pipeline = new SimpleCommandPipeline(_container, handlers);

			pipeline.EnablePublication();
		}

		private void configureContainer()
		{
			_container = new WindsorContainer();

			_container.Register(Classes.FromAssemblyInThisApplication()
				.BasedOn<ApiController>()
				.LifestylePerWebRequest());

			// set up web api di
			GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(_container.Kernel);
			GlobalConfiguration.Configuration.Services.Replace(
				typeof(IHttpControllerActivator),
				new WindsorControllerActivator(_container));

			// set up mvc di
			_container.Register(Classes.FromThisAssembly()
				.BasedOn<IController>()
				.LifestyleTransient());

			var controllerFactory = new WindsorControllerFactory(_container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
		}

		private void configureQueryLocations()
		{
			var queryLocator = new QueryLocator(typeof(TestQuery).Assembly, typeof(TestQuery).Namespace);

			// TODO: Create generalized config system
			queryLocator.AddQuerySource(typeof(LocateThisQuery).Assembly, typeof(LocateThisQuery).Namespace);

			_container.Register(
				Component.For<IQueryLocator>()
					.UsingFactoryMethod(() => queryLocator)
				);
		}
	}
}
