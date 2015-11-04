using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using BrockAllen.MembershipReboot.Ef.Migrations;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Fulcrum.Common;
using FulcrumSeed.Components;
using FulcrumSeed.Components.UserAccounts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Components.UserAccounts.Domain.Services;
using FulcrumSeed.Infrastructure.Identity;
using FulcrumSeed.Infrastructure.Membership;
using FulcrumSeed.WebAuth;
using IdentityManager.Core.Logging;
using IdentityManager.Core.Logging.LogProviders;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Services.InMemory;
using log4net.Config;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace FulcrumSeed.WebAuth
{
	public partial class Startup : ILoggingSource
	{
		private IWindsorContainer _container;

		public void Configuration(IAppBuilder app)
		{
			XmlConfigurator.Configure();

			Database.SetInitializer(new MigrateDatabaseToLatestVersion<DefaultMembershipRebootDatabase, Configuration>());

			LogProvider.SetCurrentLogProvider(new Log4NetLogProvider());

			_container = new WindsorContainer();

			var httpConfig = new HttpConfiguration();

			app.UseCors(CorsOptions.AllowAll);

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

			var factory = getFactory();

			// TODO: make SiteName configurable
			// TODO: make RequiresSsl configurable
			// TODO: make IssuerUri configurable
			// TODO: make PublicOrigin configurable
			var options = new IdentityServerOptions
			{
				SiteName = "FulcrumAPI",
				IssuerUri = "http://www.fulcrum-seed.local",
				PublicOrigin = "http://www.fulcrum-seed.local",
				RequireSsl = false,

				SigningCertificate = getCert(),
				Factory = factory,
				Endpoints = new EndpointOptions()
				{
					EnableUserInfoEndpoint = true,
					EnableDiscoveryEndpoint = true,
					EnableAuthorizeEndpoint = true,
					EnableTokenEndpoint = true,
					EnableTokenRevocationEndpoint = true
				},
			};

			app.UseIdentityServer(options);

			seedUserData();
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
				UserService = new Registration<IUserService, MembershipUserService>()
			};

			factory.Register(new Registration<AppUserService>());
			factory.Register(new Registration<UserAccountRepository>());
			factory.Register(new Registration<UserGroupRepository>());
			factory.Register(new Registration<UserAccountService<AppUser>>());
			factory.Register(new Registration<UserAccountRepository>());
			factory.Register(new Registration<IUserAccountRepository<AppUser>>(r => new UserAccountRepository(new SeedDbContext())));
			factory.Register(new Registration<SeedDbContext>(resolver => new SeedDbContext()));
			factory.Register(new Registration<UserGroupService>());
			factory.Register(new Registration<DbContextUserAccountRepository<SeedDbContext, AppUser>>());
			factory.Register(new Registration<DbContextGroupRepository<SeedDbContext, UserGroup>>());
			factory.Register(new Registration<MembershipConfig>(MembershipConfig.Config));
			factory.Register(new Registration<MembershipRebootConfiguration<AppUser>>(new MembershipRebootConfiguration<AppUser>()));

			var scopeStore = new InMemoryScopeStore(Scopes.Get());

			factory.ScopeStore = new Registration<IScopeStore>(resolver => scopeStore);

			var clientStore = new InMemoryClientStore(Clients.Get());

			factory.ClientStore = new Registration<IClientStore>(resolver => clientStore);

			factory.CorsPolicyService =
				new Registration<ICorsPolicyService>(new DefaultCorsPolicyService { AllowAll = true });

			return factory;
		}

		// TODO: move this to FulcrumSeed/Migrations/Configuration.cs
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
