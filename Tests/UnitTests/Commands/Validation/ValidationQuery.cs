using Fulcrum.Core;

namespace UnitTests.Commands.Validation
{
	public class ValidationQuery : ICommandValidationQuery<SchemaValidationCommand>
	{
		public CommandValidationResult ValidateCommand(SchemaValidationCommand command)
		{
			return new CommandValidationResult(true);
		}
	}
}
