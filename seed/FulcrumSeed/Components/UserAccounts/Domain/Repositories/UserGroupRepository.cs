using System.Collections.Generic;
using System.Linq;
using BrockAllen.MembershipReboot.Ef;
using Fulcrum.Core.Concepts;
using FulcrumSeed.Components.UserAccounts.Domain.Entities;

namespace FulcrumSeed.Components.UserAccounts.Domain.Repositories
{
	public class UserGroupRepository : DbContextGroupRepository<SeedDbContext, UserRole>, IRepository
	{
		private readonly SeedDbContext _db;

		public UserGroupRepository(SeedDbContext db)
			: base(db)
		{
			_db = db;
		}

		public IList<UserRole> ListAll()
		{
			return _db.Groups.ToList();
		}
	}
}