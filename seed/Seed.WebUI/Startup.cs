using System;
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
using BrockAllen.MembershipReboot.Ef.Migrations;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter.Unofficial;
using Fulcrum.Common;
using Fulcrum.Common.Web;
using Fulcrum.Runtime;
using FulcrumSeed.Components;
using FulcrumSeed.Components.UserAccounts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Components.UserAccounts.Domain.Services;
using FulcrumSeed.Infrastructure.IdentityServer3;
using FulcrumSeed.Infrastructure.MembershipReboot;
using FulcrumSeed.WebUI;
using IdentityManager.Configuration;
using IdentityManager.Core.Logging;
using IdentityManager.Core.Logging.LogProviders;
using IdentityServer3.AccessTokenValidation;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Services.InMemory;
using log4net.Config;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
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
			
			LogProvider.SetCurrentLogProvider(new Log4NetLogProvider());

			_container = new WindsorContainer();

			var httpConfig = new HttpConfiguration();

			FulcrumSetup.ConfigureContainer<SeedDbContext, SeedSettings>(_container);
			FulcrumSetup.ConfigureCommandsAndHandlers(_container);
			FulcrumSetup.ConfigureQueries(_container);

			app.UseCors(CorsOptions.AllowAll);

			configureMvc();

			configureWebApi(httpConfig);

			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
			{
			});

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

			app.Map("/identity",
				idsrvApp =>
				{
					var factory = getFactory();

					// TODO: make RequiresSsl configurable
					// TODO: make IssuerUri configurable
					// TODO: make PublicOrigin configurable
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

			app.Map("/api", apiApp =>
			{
				//var factory = new IdentityManagerServiceFactory();
				//factory.Configure("MembershipReboot");

				//apiApp.UseIdentityManager(new IdentityManagerOptions()
				//{
				//	Factory = factory,
				//});

				apiApp.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
				{
					Authority = "http://www.fulcrum-seed.local/identity",
					IssuerName = "FulcrumAPI",
					SigningCertificate = getCert(),
					RequiredScopes = new[] { "FulcrumApiScope" },
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

		private X509Certificate2 getCert()
		{
			return new X509Certificate2(
				string.Format(@"{0}\bin\Properties\Fulcrum.pfx", AppDomain.CurrentDomain.BaseDirectory), "password123");
		}

		private IdentityServerServiceFactory getFactory()
		{
			var factory = new IdentityServerServiceFactory
			{
				UserService = new IdentityServer3.Core.Configuration.Registration<IUserService, MembershipUserService>()
			};

			factory.Register(new IdentityServer3.Core.Configuration.Registration<AppUserService>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<UserAccountRepository>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<UserGroupRepository>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<UserAccountService<AppUser>>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<UserAccountRepository>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<IUserAccountRepository<AppUser>>(r => new UserAccountRepository(new SeedDbContext())));
			factory.Register(new IdentityServer3.Core.Configuration.Registration<SeedDbContext>(resolver => new SeedDbContext()));
			factory.Register(new IdentityServer3.Core.Configuration.Registration<UserGroupService>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<DbContextUserAccountRepository<SeedDbContext, AppUser>>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<DbContextGroupRepository<SeedDbContext, UserGroup>>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<MembershipConfig>(MembershipConfig.Config));
			factory.Register(new IdentityServer3.Core.Configuration.Registration<MembershipRebootConfiguration<AppUser>>(new MembershipRebootConfiguration<AppUser>()));

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
			var svc = new AppUserService(MembershipConfig.Config, new UserAccountRepository(new SeedDbContext()));

			if (svc.GetByUsername("testAdmin@example.com") == null)
			{
				var admin = svc.CreateAccount("testAdmin@example.com", "password", "testAdmin@example.com");

				svc.AddClaim(admin.ID, ClaimTypes.Role, UserRoles.Admin);
			}
		}
	}
}
