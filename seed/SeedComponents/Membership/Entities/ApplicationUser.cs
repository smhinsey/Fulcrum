using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot.Relational;

namespace SeedComponents.Membership.Entities
{
	public class ApplicationUser : RelationalUserAccount
	{
		[Display(Name = "First Name")]
		public virtual string FirstName { get; set; }
		[Display(Name = "Last Name")]
		public virtual string LastName { get; set; }
	}
}