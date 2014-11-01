using System.Linq;
using Castle.Windsor;
using Fulcrum.Core;
using Fulcrum.Runtime.CommandPipeline;
using Tests.Unit.Commands.Pipeline;
using Xunit;

namespace Tests.Unit.Commands
{
	public class CommandPipelineTests
	{
		[Fact]
		public void Executes_command_handler_which_throws()
		{
			var container = new WindsorContainer();

			var handlers = PipelineInstaller.InstallHandlers(container, GetType().Assembly);

			var pipeline = new SimpleCommandPipeline(container, handlers);

			pipeline.EnablePublication();
			var record = pipeline.Publish(new PingPipelineCommand());

			Assert.Equal(CommandPublicationStatus.Failed, record.Status);
		}

		[Fact]
		public void Installs_handlers()
		{
			var container = new WindsorContainer();

			var handlers = PipelineInstaller.InstallHandlers(container, GetType().Assembly);

			Assert.Equal(1, handlers.Count(h => h.GetType() == typeof(PingPongHandler)));
		}
	}
}
