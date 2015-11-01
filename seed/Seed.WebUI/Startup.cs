using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Http.Controllers;
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
using Fulcrum.Runtime;
using IdentityManager.Configuration;
using IdentityManager.Core.Logging;
using IdentityManager.Core.Logging.LogProviders;
using IdentityManager.Logging;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Services.InMemory;
using log4net.Config;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Owin;
using Seed.WebUI;
using SeedComponents;
using SeedComponents.Infrastructure.Identity;
using SeedComponents.Membership;
using SeedComponents.Membership.Entities;
using SeedComponents.Membership.Extensions;
using SeedComponents.Membership.Repositories;
using SeedComponents.Membership.Services;

[assembly: OwinStartup(typeof(Startup))]

namespace Seed.WebUI
{
	public partial class Startup : ILoggingSource
	{
		private IWindsorContainer _container;

		public void Configuration(IAppBuilder app)
		{
			XmlConfigurator.Configure();

			Database.SetInitializer(new MigrateDatabaseToLatestVersion<DefaultMembershipRebootDatabase, BrockAllen.MembershipReboot.Ef.Migrations.Configuration>());

			LogProvider.SetCurrentLogProvider(new Log4NetLogProvider());

			_container = new WindsorContainer();

			var httpConfig = new HttpConfiguration();

			FulcrumSetup.ConfigureContainer<SeedDbContext, UserSystemSettings>(_container);
			FulcrumSetup.ConfigureCommandsAndHandlers(_container);
			FulcrumSetup.ConfigureQueries(_container);

			app.UseCors(CorsOptions.AllowAll);

			configureMvc();

			configureWebApi(httpConfig);

			app.UseWebApi(httpConfig);

			configureIdentityServerAndMembershipReboot(app);
		}

		private void configureIdentityServerAndMembershipReboot(IAppBuilder app)
		{
			var config = new MembershipRebootConfiguration
			{
				RequireAccountVerification = false
			};

			//_container.Register(
			//	Component.For<MembershipRebootConfiguration>()
			//					 .Instance(config)
			//					 .LifestyleSingleton());

			//_container.Register(
			//	Component.For<DefaultMembershipRebootDatabase>()
			//					 .LifestylePerWebRequest());

			//_container.Register(
			//	Component.For<IUserAccountRepository>()
			//					 .ImplementedBy<DefaultUserAccountRepository>()
			//					 .LifestylePerWebRequest());

			//_container.Register(
			//	Component.For<UserAccountService>()
			//					 .LifestylePerWebRequest());

			app.Use(async (ctx, next) =>
			              {
				              using (_container.BeginScope())
				              {
					              ctx.Environment.SetUserAccountService(_container.Resolve<UserAccountService>);

					              await next();
				              }
			              });

			var connectionString = "MembershipReboot";

			app.Map("/admin",
				adminApp =>
				{
					var factory = new IdentityManagerServiceFactory();

					factory.Configure(connectionString);

					adminApp.UseIdentityManager(new IdentityManagerOptions()
					{
						Factory = factory
					});
				});

			app.Map("/identity",
				idsrvApp =>
				{
					var factory = getFactory();

					factory.ConfigureCustomUserService(connectionString);

					// TODO: make RequiresSsl configurable
					// TODO: make IssuerUri configurable
					// TODO: make PublicOrigin configurable
					// TODO: make ??? configurable
					var options = new IdentityServerOptions
					{
						SiteName = "FulcrumAPI",
						IssuerUri = "http://www.fulcrum-seed.local",
						SigningCertificate = getCert(),
						Factory = factory,
						PublicOrigin = "http://www.fulcrum-seed.local",
						RequireSsl = false,
					};

					idsrvApp.UseIdentityServer(options);
				});

			//seedUserData();
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

			_container.Register(Classes.FromAssemblyInThisApplication()
			                           .BasedOn<IHttpController>()
			                           .LifestylePerWebRequest());

			GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(_container.Kernel);

			GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorControllerActivator(_container));

			config.DependencyResolver = new WindsorDependencyResolver(_container.Kernel);

			config.Services.Replace(typeof(IHttpControllerActivator), new WindsorControllerActivator(_container));
		}

		private X509Certificate2 getCert()
		{
			return new X509Certificate2(
				string.Format(@"{0}\bin\Properties\Fulcrum.pfx", AppDomain.CurrentDomain.BaseDirectory), "password123");
		}

		private IdentityServerServiceFactory getFactory()
		{
			var factory = new IdentityServerServiceFactory();

			factory.Register(new IdentityServer3.Core.Configuration.Registration<ApplicationAccountService>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<CustomGroupService>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<CustomUserRepository>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<CustomGroupRepository>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<SeedDbContext>
				(resolver => new SeedDbContext()));
			
			factory.Register(new IdentityServer3.Core.Configuration.Registration<MembershipConfig>(MembershipConfig.Config));

			var scopeStore = new InMemoryScopeStore(Scopes.Get());

			factory.ScopeStore = new IdentityServer3.Core.Configuration.Registration<IScopeStore>(resolver => scopeStore);

			var clientStore = new InMemoryClientStore(Clients.Get());

			factory.ClientStore = new IdentityServer3.Core.Configuration.Registration<IClientStore>(resolver => clientStore);

			factory.CorsPolicyService =
				new IdentityServer3.Core.Configuration.Registration<ICorsPolicyService>(new DefaultCorsPolicyService { AllowAll = true });

			return factory;
		}

		// TODO: once migrations are set up, move this to Configuration.cs
		private void seedUserData()
		{
			using (_container.BeginScope())
			{
				var svc = _container.Resolve<UserAccountService<ApplicationUser>>();

				if (svc.GetByUsername("testAdmin") == null)
				{
					var admin = svc.CreateAccount("testAdmin", "password", "testAdmin@example.com");

					svc.AddClaim(admin.ID, ClaimTypes.Role, UserRoles.Admin);
				}
			}
		}
	}
}
