using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace SeedComponents.Membership
{
	// TODO: lock down CORS origins
	public class AuthProvider : OAuthAuthorizationServerProvider
	{
		public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

			var svc = context.OwinContext.Environment.GetUserAccountService<UserAccount>();

			UserAccount user;

			if (svc.Authenticate("users", context.UserName, context.Password, out user))
			{
				var claims = user.GetAllClaims();

				var id = new ClaimsIdentity(claims, "MembershipReboot");
				context.Validated(id);
			}

			return base.GrantResourceOwnerCredentials(context);
		}

		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

			string cid, csecret;

			if (context.TryGetBasicCredentials(out cid, out csecret))
			{
				var svc = context.OwinContext.Environment.GetUserAccountService<UserAccount>();
				if (svc.Authenticate("clients", cid, csecret))
				{
					context.Validated();
				}
			}

			return Task.FromResult<object>(null);
		}

		public override Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
		{
			context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

			if (context.TokenRequest.IsResourceOwnerPasswordCredentialsGrantType)
			{
				var svc = context.OwinContext.Environment.GetUserAccountService<UserAccount>();
				var client = svc.GetByUsername("clients", context.ClientContext.ClientId);
				var scopes = context.TokenRequest.ResourceOwnerPasswordCredentialsGrant.Scope;

				if (scopes.All(scope => client.HasClaim("scope", scope)))
				{
					context.Validated();
				}
			}

			return Task.FromResult<object>(null);
		}
	}
}
