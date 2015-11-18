using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Fulcrum.Common;
using FulcrumSeed.Components;
using FulcrumSeed.Components.UserAccounts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;
using FulcrumSeed.Components.UserAccounts.Domain.Repositories;
using FulcrumSeed.Components.UserAccounts.Domain.Services;
using FulcrumSeed.Infrastructure.IdentityServer3;
using FulcrumSeed.Infrastructure.MembershipReboot;
using FulcrumSeed.Infrastructure.MembershipReboot.Extensions;
using FulcrumSeed.WebAuth;
using IdentityManager.Api.Models.Controllers;
using IdentityManager.Configuration;
using IdentityManager.Core.Logging;
using IdentityManager.Core.Logging.LogProviders;
using IdentityModel.Client;
using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Services.InMemory;
using log4net.Config;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using AuthenticationOptions = IdentityServer3.Core.Configuration.AuthenticationOptions;
using Configuration = BrockAllen.MembershipReboot.Ef.Migrations.Configuration;

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

			setupContainer(config);

			app.Use(async (ctx, next) =>
			              {
				              ctx.Environment.SetUserAccountService(_container.Resolve<UserAccountService>);

				              await next();
			              });

			// TODO: make configurable
			var siteName = "Fulcrum API";
			// TODO: make configurable
			var issuerUri = "http://www.fulcrum-seed.local";
			// TODO: make configurable
			var publicOrigin = "http://www.fulcrum-seed.local";
			// TODO: make configurable
			var requireSsl = false;

			var options = new IdentityServerOptions
			{
				SiteName = siteName,
				IssuerUri = issuerUri,
				PublicOrigin = publicOrigin,
				RequireSsl = requireSsl,
				SigningCertificate = getCert(),
				Factory = getServerFactory(),
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

		private void setupContainer(MembershipRebootConfiguration config)
		{
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

			_container.Register(
				Component.For<MetaController>()
								 .LifestylePerWebRequest());
		}

		private X509Certificate2 getCert()
		{
			return new X509Certificate2(
				string.Format(@"{0}\bin\Properties\Fulcrum.pfx", AppDomain.CurrentDomain.BaseDirectory), "password123");
		}

		private IdentityServerServiceFactory getServerFactory()
		{
			var factory = new IdentityServerServiceFactory
			{
				UserService = new IdentityServer3.Core.Configuration.Registration<IUserService, MembershipUserService>()
			};

			factory.Register(new IdentityServer3.Core.Configuration.Registration<MetaController>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<IClaimsProvider>(typeof(CustomClaimsProvider)));
			factory.Register(new IdentityServer3.Core.Configuration.Registration<AppUserService>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<UserAccountRepository>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<UserGroupRepository>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<UserAccountService<AppUser>>(r=>new AppUserService(MembershipConfig.Config, new UserAccountRepository(new SeedDbContext()))));
			factory.Register(new IdentityServer3.Core.Configuration.Registration<UserAccountRepository>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<IUserAccountRepository<AppUser>>(r => new UserAccountRepository(new SeedDbContext())));
			factory.Register(new IdentityServer3.Core.Configuration.Registration<SeedDbContext>(resolver => new SeedDbContext()));
			factory.Register(new IdentityServer3.Core.Configuration.Registration<UserGroupService>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<DbContextUserAccountRepository<SeedDbContext, AppUser>>());
			factory.Register(new IdentityServer3.Core.Configuration.Registration<DbContextGroupRepository<SeedDbContext, UserClaimGroup>>());
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

		// TODO: move this to FulcrumSeed/Migrations/Configuration.cs
		private void seedUserData()
		{
			var svc = new AppUserService(MembershipConfig.Config, new UserAccountRepository(new SeedDbContext()));

			var user = svc.GetByUsername("testAdmin@example.com");

			if (user == null)
			{
				var admin = svc.CreateAccount("testAdmin@example.com", "password", "testAdmin@example.com");

				svc.AddClaim(admin.ID, ClaimTypes.Role, UserRoles.Admin);
				svc.AddClaim(admin.ID, ClaimTypes.Role, UserRoles.AuthenticatedUser);
			}
			else
			{
				var claims = svc.MapClaims(user);

				svc.RemoveClaims(user.ID, new UserClaimCollection(claims));

				svc.AddClaim(user.ID, ClaimTypes.Role, UserRoles.Admin);
				svc.AddClaim(user.ID, ClaimTypes.Role, UserRoles.AuthenticatedUser);
			}
		}
	}
}
