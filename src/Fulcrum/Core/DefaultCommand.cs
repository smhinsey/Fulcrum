using System;
using Newtonsoft.Json;

namespace Fulcrum.Core
{
	public abstract class DefaultCommand : ICommand
	{
		protected DefaultCommand()
		{
			PublicationRecordId = Guid.Empty;
		}

		[JsonIgnore]
		public Guid PublicationRecordId { get; private set; }

		public void AssignPublicationRecordId(Guid id)
		{
			PublicationRecordId = id;
		}
	}
}