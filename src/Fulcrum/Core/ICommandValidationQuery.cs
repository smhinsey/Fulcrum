namespace Fulcrum.Core
{
	public interface ICommandValidationQuery<in TCommand> : ICommandValidationQuery
		where TCommand : ICommand
	{
		CommandValidationResult ValidateCommand(TCommand command);
	}

	public interface ICommandValidationQuery : IQuery
	{
	}
}
