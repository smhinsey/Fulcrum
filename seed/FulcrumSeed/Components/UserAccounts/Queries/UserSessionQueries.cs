using System.Collections.Generic;
using Fulcrum.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

namespace FulcrumSeed.Components.UserAccounts.Queries
{
	public class UserSessionQueries : IQuery
	{
		private readonly IRefreshTokenStore _tokenStore;

		public UserSessionQueries(IRefreshTokenStore tokenStore)
		{
			_tokenStore = tokenStore;
		}

		public IEnumerable<ITokenMetadata> GetAll()
		{
			return _tokenStore.GetAllAsync("").Result;
		}
	}
}