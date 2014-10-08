using System.ComponentModel.DataAnnotations;
using Fulcrum.Core;

namespace Tests.Unit.Commands.DescribeTheseCommands
{
	public class DescribeThisValidatedCommand : ICommand
	{
		[RegularExpression(".@")]
		public string EmailWithPattern { get; set; }

		[Required]
		[Range(18, 100)]
		public int RequiredAgeWithMinAndMax { get; set; }

		[Required]
		public string RequiredFirstName { get; set; }
	}
}
