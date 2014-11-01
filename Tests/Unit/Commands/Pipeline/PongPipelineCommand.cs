using Fulcrum.Core;

namespace Tests.Unit.Commands.Pipeline
{
	public class PongPipelineCommand : DefaultCommand
	{
		public string PlayerName { get; set; }
	}
}