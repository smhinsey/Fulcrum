using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Fulcrum.Core;
using FulcrumSeed.Components.UserAccounts.Queries.Projections;
using IdentityServer3.EntityFramework;
using IdentityServer3.EntityFramework.Entities;

namespace FulcrumSeed.Components.UserAccounts.Queries
{
	public class UserSessionQueries : IQuery
	{
		private readonly IOperationalDbContext _db;

		public UserSessionQueries()
		{
			_db = new OperationalDbContext(ConfigurationManager.ConnectionStrings["FulcrumSeedDb"].ConnectionString);
		}

		public IList<TokenProjection> GetAll()
		{
			var query = _db.Tokens.Where(t => t.TokenType == TokenType.RefreshToken);

			var projection = query.Select(t => new TokenProjection
			{
				ClientId = t.ClientId,
				Expiry = t.Expiry,
				RawJson = t.JsonCode
			});

			return projection.ToList();
		}
	}
}
