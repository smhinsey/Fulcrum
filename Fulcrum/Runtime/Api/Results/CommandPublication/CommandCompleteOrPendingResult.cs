using System;
using System.Collections.Generic;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results.CommandPublication
{
	public class CommandCompleteOrPendingResult
	{
		public CommandCompleteOrPendingResult(ICommandPublicationRecord record)
		{
			Created = record.Created;
			Id = record.Id;
			Status = record.Status;
			Updated = record.Updated;
			CommandName = record.PortableCommand.ClrTypeName;
			Links = new List<JsonLink>()
			{
				new JsonLink(string.Format("/commands/publication-registry/{0}", Id), "details"),
				new JsonLink("/commands/", "home")
			};
		}

		public string CommandName { get; set; }

		public DateTime Created { get; private set; }

		public Guid Id { get; private set; }

		public IList<JsonLink> Links { get; private set; }

		public CommandPublicationStatus Status { get; private set; }

		public DateTime? Updated { get; private set; }
	}
}
