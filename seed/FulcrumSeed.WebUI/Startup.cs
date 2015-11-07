using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter.Unofficial;
using Fulcrum.Common;
using Fulcrum.Common.Web;
using Fulcrum.Runtime;
using FulcrumSeed.Components;
using FulcrumSeed.WebUI;
using log4net.Config;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace FulcrumSeed.WebUI
{
	public partial class Startup : ILoggingSource
	{
		private IWindsorContainer _container;

		public void Configuration(IAppBuilder app)
		{
			XmlConfigurator.Configure();

			_container = new WindsorContainer();

			FulcrumSetup.ConfigureContainer<SeedDbContext, SeedSettings>(_container);
			FulcrumSetup.ConfigureCommandsAndHandlers(_container);
			FulcrumSetup.ConfigureQueries(_container);

			app.UseCors(CorsOptions.AllowAll);

			configureMvc();
		}

		// TODO: move to CommonAppSetup
		private void configureMvc()
		{
			_container.Register(Classes.FromThisAssembly()
			                           .BasedOn<IController>()
			                           .LifestyleTransient());

			var controllerFactory = new WindsorControllerFactory(_container.Kernel);

			ControllerBuilder.Current.SetControllerFactory(controllerFactory);

			DependencyResolver.SetResolver(new WindsorServiceLocator(_container));
		}
	}
}
