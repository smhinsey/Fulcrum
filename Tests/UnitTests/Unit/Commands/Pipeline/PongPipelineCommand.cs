using System.ComponentModel.DataAnnotations;
using Fulcrum.Core;

namespace UnitTests.Unit.Commands.Pipeline
{
	public class PongPipelineCommand : DefaultCommand
	{
		[Required]
		public string PlayerName { get; set; }

		[Required]
		[Range(18, 100)]
		public int NumberToBeValidated { get; set; }
	}
}