using Fulcrum.Core;

namespace Tests.Unit.Commands.Pipeline
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
