using Fulcrum.Core;

namespace Examples.UserProfileBC.Errors
{
	public class UsernameAlreadyInUse : Error
	{
		public UsernameAlreadyInUse(ICommand relatedCommand) : base(relatedCommand)
		{
		}
	}
}
