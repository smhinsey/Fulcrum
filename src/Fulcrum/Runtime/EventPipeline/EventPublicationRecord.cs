using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using Fulcrum.Core;

namespace Fulcrum.Runtime.EventPipeline
{
	public class EventPublicationRecord : IEventPublicationRecord
	{
		public EventPublicationRecord()
		{
		}

		public EventPublicationRecord(IEvent ev)
		{
			Status = EventPublicationStatus.Unpublished;
			Id = ev.PublicationRecordId;
			PortableEvent = new PortableEvent(ev, EventSchemaGenerator.GenerateSchema(ev.GetType()));
			Created = DateTime.UtcNow;
			QueryReferences = new EditableList<IdentifierQueryReference>();
		}

		public DateTime Created { get; private set; }

		public string ErrorDetails { get; set; }

		public string ErrorHeadline { get; set; }

		public PortableEvent PortableEvent { get; private set; }

		public IList<IdentifierQueryReference> QueryReferences { get; set; }

		public EventPublicationStatus Status { get; set; }

		public DateTime? Updated { get; set; }

		public Guid Id { get; private set; }
	}
}
