namespace Fulcrum.Core
{
	public interface ICommandValidationQuery<in TCommand>
		where TCommand : ICommand
	{
		CommandValidationResult ValidateCommand(TCommand command);
	}
}
