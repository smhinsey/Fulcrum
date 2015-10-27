using Fulcrum.Core;

namespace UnitTests.Commands.Pipeline
{
	public class PingPongHandler :
		ICommandHandler<PingPipelineCommand>
	{
		public void Handle(PingPipelineCommand command)
		{
			throw new PingPongError();
		}
	}
}
