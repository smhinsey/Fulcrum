using ApprovalTests;
using Fulcrum.Core;
using Fulcrum.Runtime;
using UnitTests.Unit.Commands.PortableCommands;
using Xunit;

namespace UnitTests.Unit.Commands
{
	public class PortableCommandTests
	{
		[Fact]
		public void Verify_json_command()
		{
			var portableCommand = getTestCommand();

			Approvals.Verify(portableCommand.CommandJson);
		}

		[Fact]
		public void Verify_json_command_schema()
		{
			var portableCommand = getTestCommand();

			Approvals.Verify(portableCommand.CommandJsonSchema);
		}

		private static PortableCommand getTestCommand()
		{
			var sampleCommand = new PortabilityCommand
			{
				MarketingSlogan = "So portable, you can smell it!"
			};

			var schema = CommandSchemaGenerator.GenerateSchema(sampleCommand.GetType());

			var portableCommand = new PortableCommand(sampleCommand, schema);
			return portableCommand;
		}
	}
}
