using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter.Unofficial;
using Fulcrum.Common;
using Fulcrum.Common.Web;
using Fulcrum.Runtime.TiltedGlobe.Runtime;
using IdentityManager.Configuration;
using log4net.Config;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using Seed.WebUI;
using SeedComponents;
using SeedComponents.Membership;
using SeedComponents.Membership.Extensions;

[assembly: OwinStartup(typeof(Startup))]

namespace Seed.WebUI
{
	public partial class Startup : ILoggingSource
	{
		private IWindsorContainer _container;

		public void Configuration(IAppBuilder app)
		{
			XmlConfigurator.Configure();

			_container = new WindsorContainer();

			configureMembershipReboot(app);

			var httpConfig = new HttpConfiguration();

			var oauthServerConfig = new OAuthAuthorizationServerOptions
			{
				AllowInsecureHttp = true,
				Provider = new AuthProvider(),
				TokenEndpointPath = new PathString("/token")
			};

			app.UseOAuthAuthorizationServer(oauthServerConfig);

			var oauthConfig = new OAuthBearerAuthenticationOptions
			{
				AuthenticationMode = AuthenticationMode.Active,
				AuthenticationType = "Bearer"
			};

			app.UseOAuthBearerAuthentication(oauthConfig);

			CommonAppSetup.ConfigureContainer<UserSystemDb, UserSystemSettings>(_container);
			CommonAppSetup.ConfigureCommandsAndHandlers(_container);
			CommonAppSetup.ConfigureQueries(_container);

			configureWebApi(httpConfig);

			app.UseCors(CorsOptions.AllowAll);

			app.UseWebApi(httpConfig);

			configureMvc();
		}

		private void configureMembershipReboot(IAppBuilder app)
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
				         .LifestylePerWebRequest());

			app.Use(async (ctx, next) =>
			              {
				              using (_container.BeginScope())
				              {
					              ctx.Environment.SetUserAccountService(_container.Resolve<UserAccountService>);

					              await next();
				              }
			              });

			var connectionString = "MembershipReboot";

			app.Map("/admin", adminApp =>
			                  {
				                  var factory = new IdentityManagerServiceFactory();
				                  factory.Configure(connectionString);

				                  adminApp.UseIdentityManager(new IdentityManagerOptions()
				                  {
					                  Factory = factory
				                  });
			                  });

			seedUserData();
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

		// TODO: once migrations are set up, move this there
		private void seedUserData()
		{
			using (_container.BeginScope())
			{
				var svc = _container.Resolve<UserAccountService>();

				if (svc.GetByUsername("users", "testAdmin") == null)
				{
					var admin = svc.CreateAccount("users", "testAdmin", "password", "testAdmin@example.com");

					svc.AddClaim(admin.ID, ClaimTypes.Role, UserRoles.Admin);
				}
			}
		}
	}
}
