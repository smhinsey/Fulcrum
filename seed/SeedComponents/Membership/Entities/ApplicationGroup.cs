using BrockAllen.MembershipReboot;

namespace SeedComponents.Membership.Entities
{
	public class ApplicationGroup : RelationalGroup
	{
		public virtual string Description { get; set; }
	}
}