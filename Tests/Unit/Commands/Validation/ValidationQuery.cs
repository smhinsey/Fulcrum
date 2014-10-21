using Fulcrum.Core;

namespace Tests.Unit.Commands.Validation
{
	public class ValidationQuery : ICommandValidationQuery<SchemaValidationCommand>
	{
		public CommandValidationResult ValidateCommand(SchemaValidationCommand command)
		{
			return new CommandValidationResult(true);
		}
	}
}
