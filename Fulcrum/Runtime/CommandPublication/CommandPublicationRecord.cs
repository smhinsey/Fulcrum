using System;
using Fulcrum.Common.Registry;

namespace Fulcrum.Runtime
{
	public class CommandPublicationRecord : IRecord
	{
		private CommandPublicationRecord()
		{
		}

		public CommandPublicationStatus Status { get; private set; }

		public Guid Id { get; set; }
	}
}
