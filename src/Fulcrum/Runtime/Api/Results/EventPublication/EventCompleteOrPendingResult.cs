using System;
using System.Collections.Generic;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results.EventPublication
{
	public class EventCompleteOrPendingResult
	{
		public EventCompleteOrPendingResult(IEventPublicationRecord record)
		{
			Created = record.Created;
			Id = record.Id;
			Status = record.Status;
			Updated = record.Updated;
			EventName = record.PortableEvent.ClrTypeName;
			QueryReferences = record.QueryReferences;

			Links = new List<JsonLink>()
			{
				new JsonLink(string.Format("/api/events/publication-registry/{0}", Id), "details"),
				new JsonLink("/api/events/", "home")
			};

			foreach (var reference in QueryReferences)
			{
				var queryUrl = string.Format("/api/queries/{0}/results?id={1}",
					reference.QueryName, reference.QueryParameter);

				Links.Add(new JsonLink(queryUrl, "query-reference"));
			}
		}

		public DateTime Created { get; private set; }

		public string EventName { get; set; }

		public Guid Id { get; private set; }

		public IList<JsonLink> Links { get; private set; }

		public IList<IdentifierQueryReference> QueryReferences { get; private set; }

		public EventPublicationStatus Status { get; private set; }

		public DateTime? Updated { get; private set; }
	}
}
