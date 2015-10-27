using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SeedComponents
{
	public class UserSystemDb : DbContext
	{
		public UserSystemDb() : base("UserSystemDb")
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}
