using Fulcrum.Core;

namespace Fulcrum.Tests.ExampleUserSystem.Errors
{
	public class UsernameAlreadyInUse : Error
	{
		public UsernameAlreadyInUse(ICommand relatedCommand) : base(relatedCommand)
		{
		}
	}
}