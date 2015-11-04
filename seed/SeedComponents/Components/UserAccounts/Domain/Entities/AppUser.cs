using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot.Relational;

namespace FulcrumSeed.Components.UserAccounts.Domain.Entities
{
	public class AppUser : RelationalUserAccount
	{
		// TODO: move these properties to a linked UserProfile type
		[Display(Name = "First Name")]
		public virtual string FirstName { get; set; }
		[Display(Name = "Last Name")]
		public virtual string LastName { get; set; }
	}
}