using System;
using System.Collections.Generic;
using System.Linq;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Core;
using Newtonsoft.Json;

namespace Fulcrum.Runtime.Api.Results.EventPublication
{
	public class DetailedEventPublicationRecordResult
	{
		public DetailedEventPublicationRecordResult(IEventPublicationRecord record)
		{
			Created = record.Created;
			Id = record.Id;
			Status = record.Status;
			Updated = record.Updated;
			EventName = record.PortableEvent.ClrTypeName;
			Links = new List<JsonLink>()
			{
				new JsonLink("/api/events/", "home")
			};
			OriginalEvent = JsonConvert.DeserializeObject(record.PortableEvent.EventJson);
			EventSchema = JsonConvert.DeserializeObject(record.PortableEvent.EventJsonSchema);
			ErrorDetails = record.ErrorDetails;
			ErrorHeadline = record.ErrorHeadline;

			foreach (var reference in record.QueryReferences)
			{
				var queryUrl = string.Format("/api/queries/{0}/results?id={1}",
					reference.QueryName, string.Join("&amp;", reference.Parameters.Select(x => x.Name + "=" + x.Value).ToArray()));

				Links.Add(new JsonLink(queryUrl, "query-reference"));
			}
		}

		public DateTime Created { get; private set; }

		public string ErrorDetails { get; private set; }

		public string ErrorHeadline { get; private set; }

		public string EventName { get; set; }

		public object EventSchema { get; private set; }

		public Guid Id { get; private set; }

		public IList<JsonLink> Links { get; private set; }

		public object OriginalEvent { get; private set; }

		public EventPublicationStatus Status { get; private set; }

		public DateTime? Updated { get; private set; }
	}
}
