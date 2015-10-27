namespace Fulcrum.Core
{
	public interface IFormModel<out TCommand>
		where TCommand : ICommand
	{
		TCommand Convert();
	}
}
