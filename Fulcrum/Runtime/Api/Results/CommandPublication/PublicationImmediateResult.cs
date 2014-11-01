using System;
using System.Collections.Generic;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results.CommandPublication
{
	public class PublicationImmediateResult
	{
		public PublicationImmediateResult(ICommandPublicationRecord record)
		{
			Created = record.Created;
			Id = record.Id;
			Status = record.Status;
			Updated = record.Updated;
			CommandName = record.PortableCommand.ClrTypeName;
			Links = new List<JsonLink>() { new JsonLink(string.Format("command-registry/{0}", Id), "details") };
		}

		public string CommandName { get; set; }

		public DateTime Created { get; private set; }

		public Guid Id { get; private set; }

		public IList<JsonLink> Links { get; private set; }

		public CommandPublicationStatus Status { get; private set; }

		public DateTime? Updated { get; private set; }
	}
}
