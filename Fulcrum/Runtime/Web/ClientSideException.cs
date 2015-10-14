using System;

namespace Fulcrum.Runtime.Web
{
	public class ClientSideException : Exception
	{
		public ClientSideException(string message) : base(message)
		{
		}
	}
}