using Fulcrum.Core;

namespace UnitTests.Unit.Commands.Validation
{
	public class ValidationQuery : ICommandValidationQuery<SchemaValidationCommand>
	{
		public CommandValidationResult ValidateCommand(SchemaValidationCommand command)
		{
			return new CommandValidationResult(true);
		}
	}
}
