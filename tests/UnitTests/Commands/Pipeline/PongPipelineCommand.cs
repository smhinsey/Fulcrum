using System.ComponentModel.DataAnnotations;
using Fulcrum.Core;

namespace UnitTests.Commands.Pipeline
{
	public class PongPipelineCommand : DefaultCommand
	{
		[Required]
		[Range(18, 100)]
		public int NumberToBeValidated { get; set; }

		[Required]
		public string PlayerName { get; set; }
	}
}
