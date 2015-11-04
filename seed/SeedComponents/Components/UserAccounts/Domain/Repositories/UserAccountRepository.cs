using System.Collections.Generic;
using System.Linq;
using BrockAllen.MembershipReboot.Ef;
using Fulcrum.Core.Concepts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed.Components.UserAccounts.Domain.Repositories
{
	public class UserAccountRepository : DbContextUserAccountRepository<SeedDbContext, AppUser>, IRepository
	{
		private readonly SeedDbContext _db;

		public UserAccountRepository(SeedDbContext db)
			: base(db)
		{
			_db = db;
		}

		public IList<AppUser> ListAll()
		{
			return _db.Users.ToList();
		}
	}
}