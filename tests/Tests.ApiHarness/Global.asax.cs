﻿using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Fulcrum.Common.Web;
using Fulcrum.Core;
using Fulcrum.Runtime;
using Fulcrum.Runtime.Api;
using Fulcrum.Runtime.CommandPipeline;
using UnitTests.Commands.Pipeline;
using UnitTests.Commands.Validation;
using UnitTests.Queries.Location;
using UnitTests.Queries.Location.Additional;

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
			var commandLocator = new CommandLocator(typeof(SchemaTestingCommand).Assembly, typeof(SchemaTestingCommand).Namespace);

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

			var pipeline = new SimpleCommandPipeline(_container, handlers, new CommandPipelineDbContext());
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
			queryLocator.AddQuerySource(typeof(ValidationQuery).Assembly, typeof(ValidationQuery).Namespace);

			_container.Register(
				Component.For<IQueryLocator>()
				         .UsingFactoryMethod(() => queryLocator)
				);
		}
	}
}
