using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter.Unofficial;
using Fulcrum.Common;
using Fulcrum.Common.Web;
using Fulcrum.Runtime;
using FulcrumSeed.Components;
using FulcrumSeed.WebApi;
using IdentityManager.Core.Logging;
using IdentityManager.Core.Logging.LogProviders;
using IdentityServer3.AccessTokenValidation;
using log4net.Config;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace FulcrumSeed.WebApi
{
	public partial class Startup : ILoggingSource
	{
		private IWindsorContainer _container;

		public void Configuration(IAppBuilder app)
		{
			XmlConfigurator.Configure();

			LogProvider.SetCurrentLogProvider(new Log4NetLogProvider());

			_container = new WindsorContainer();

			var httpConfig = new HttpConfiguration();

			FulcrumSetup.ConfigureContainer<SeedDbContext, SeedSettings>(_container);
			FulcrumSetup.ConfigureCommandsAndHandlers(_container);
			FulcrumSetup.ConfigureQueries(_container);

			app.UseCors(CorsOptions.AllowAll);

			configureMvc();

			configureWebApi(httpConfig);

			configureIdentityServerAndMembershipReboot(app);

			app.UseWebApi(httpConfig);
		}

		private void configureIdentityServerAndMembershipReboot(IAppBuilder app)
		{
			var config = new MembershipRebootConfiguration
			{
				RequireAccountVerification = false
			};

			_container.Register(
				Component.For<MembershipRebootConfiguration>()
				         .Instance(config)
				         .LifestyleSingleton());

			_container.Register(
				Component.For<DefaultMembershipRebootDatabase>()
				         .LifestylePerWebRequest());

			_container.Register(
				Component.For<IUserAccountRepository>()
				         .ImplementedBy<DefaultUserAccountRepository>()
				         .LifestylePerWebRequest());

			_container.Register(
				Component.For<UserAccountService>()
				         .ImplementedBy<UserAccountService>()
				         .LifestylePerWebRequest());

			app.Use(async (ctx, next) =>
			              {
				              ctx.Environment.SetUserAccountService(_container.Resolve<UserAccountService>);

				              await next();
			              });

			// TODO: pull from config
			var authority = "http://www.fulcrum-seed.local/auth";

			app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
			{
				Authority = authority,
			});
		}

		// TODO: move to CommonAppSetup
		private void configureMvc()
		{
			_container.Register(Classes.FromAssemblyInThisApplication()
			                           .BasedOn<ApiController>()
			                           .LifestyleTransient());

			_container.Register(Classes.FromThisAssembly()
			                           .BasedOn<IController>()
			                           .LifestyleTransient());

			var controllerFactory = new WindsorControllerFactory(_container.Kernel);

			ControllerBuilder.Current.SetControllerFactory(controllerFactory);

			DependencyResolver.SetResolver(new WindsorServiceLocator(_container));
		}

		// TODO: move to CommonAppSetup
		private void configureWebApi(HttpConfiguration config)
		{
			var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();

			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

			_container.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter("bin"))
			                           .BasedOn<IHttpController>()
			                           .LifestylePerWebRequest());

			GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(_container.Kernel);

			GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorControllerActivator(_container));

			config.DependencyResolver = new WindsorDependencyResolver(_container.Kernel);

			config.Services.Replace(typeof(IHttpControllerActivator), new WindsorControllerActivator(_container));
		}
	}
}
