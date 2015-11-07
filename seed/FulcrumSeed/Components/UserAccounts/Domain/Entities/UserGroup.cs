using BrockAllen.MembershipReboot;

namespace FulcrumSeed.Components.UserAccounts.Domain.Entities
{
	public class UserGroup : RelationalGroup
	{
		public virtual string Description { get; set; }
	}
}