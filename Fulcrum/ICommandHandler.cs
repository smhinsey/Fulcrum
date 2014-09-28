using System.Collections.Generic;

namespace Fulcrum.Core
{
	public interface ICommandHandler<in TCommand>
		where TCommand : ICommand
	{
		IList<IEvent> Handle(TCommand command);
	}
}
