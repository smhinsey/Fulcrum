using System.Linq;
using Castle.Core;
using Castle.Windsor;
using Fulcrum.Core;
using Fulcrum.Runtime.CommandPipeline;
using UnitTests.Commands.Pipeline;
using Xunit;

namespace UnitTests.Commands
{
	public class CommandPipelineTests
	{
		[Fact(Skip="Needs to be fixed")]
		public void Executes_command_handler_which_throws()
		{
			var container = new WindsorContainer();

			container.Kernel.ComponentModelCreated += model =>
			                                          {
																									if (model.LifestyleType == LifestyleType.Undefined || model.LifestyleType == LifestyleType.PerWebRequest)
																										model.LifestyleType = LifestyleType.Transient;
			                                          };

			var handlers = PipelineInstaller.InstallHandlers(container, GetType().Assembly);

			var pipeline = new SimpleCommandPipeline(container, handlers, new CommandPipelineDbContext());

			var record = pipeline.Publish(new PingPipelineCommand());

			Assert.Equal(CommandPublicationStatus.Failed, record.Status);
		}

		[Fact]
		public void Installs_handlers()
		{
			var container = new WindsorContainer();

			container.Kernel.ComponentModelCreated += model =>
			{
				if (model.LifestyleType == LifestyleType.Undefined || model.LifestyleType == LifestyleType.PerWebRequest)
					model.LifestyleType = LifestyleType.Transient;
			};

			var handlers = PipelineInstaller.InstallHandlers(container, GetType().Assembly);

			Assert.Equal(1, handlers.Count(h => h.GetType() == typeof(PingPongHandler)));
		}
	}
}
