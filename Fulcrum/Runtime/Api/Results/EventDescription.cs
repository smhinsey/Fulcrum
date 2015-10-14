using System;
using System.Collections.Generic;
using Fulcrum.Common.JsonSchema;
using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results
{
	public class EventDescription : EventDescriptor
	{
		public EventDescription(Type eventType, bool includeDetails) : base(eventType.Name, eventType.Namespace)
		{
            EmptyModel = Activator.CreateInstance(eventType);

            var eventSchema = EventSchemaGenerator.GenerateSchema(eventType);

            Schema = new SchemaObject(eventSchema, Name, Namespace);
			
			Links = new List<JsonLink>()
			{
				new JsonLink(string.Format("/api/events/{0}/{1}/publish", Namespace, Name), "publication"),
			};

			if (includeDetails)
			{
                Links.Add(new JsonLink(string.Format("/api/events/{0}/{1}", Namespace, Name), "details"));
			}
			else
			{
                Links.Add(new JsonLink("/api/events/", "home"));
			}
		}

		public List<JsonLink> Links { get; private set; }

		public object EmptyModel { get; private set; }

		public SchemaObject Schema { get; private set; }
	}
}
