using System.ComponentModel.DataAnnotations;
using Fulcrum.Core;
using Fulcrum.Runtime;

namespace Tests.Unit.Commands.Validation
{
	[QueryValidation(typeof(ValidationQuery))]
	public class SchemaValidationCommand : DefaultCommand
	{
		[RegularExpression(".@")]
		public string EmailWithPattern { get; set; }

		[Required]
		[Range(18, 100)]
		public int RequiredAgeWithMinAndMax { get; set; }

		[Required]
		public bool RequiredFirstName { get; set; }
	}
}
