using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot.Relational;

namespace FulcrumSeed.Components.UserAccounts.Domain.Entities
{
	public class UserAccount : RelationalUserAccount
	{
		[Display(Name = "First Name")]
		public virtual string FirstName { get; set; }
		[Display(Name = "Last Name")]
		public virtual string LastName { get; set; }
	}
}