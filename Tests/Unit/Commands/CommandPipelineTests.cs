using Castle.Windsor;
using Fulcrum.Runtime.CommandPipeline;
using Tests.Unit.Commands.Pipeline;
using Xunit;

namespace Tests.Unit.Commands
{
	public class CommandPipelineTests
	{
		[Fact]
		public void Enables_without_error()
		{
			var container = new WindsorContainer();

			var pipeline = new SimpleCommandPipeline(container);

			pipeline.InstallHandlers(container, GetType().Assembly);

			pipeline.EnablePublication();

			pipeline.Publish(new PingPipelineCommand());
		}
	}
}