using Newtonsoft.Json;

namespace Fulcrum.Core
{
	/// <summary>
	///   A portable representation of a event, where portable means that the event
	///   and its schema are encoded in JSON.
	/// </summary>
	public class PortableEvent
	{
		public PortableEvent()
		{
		}

        public PortableEvent(IEvent ev, EventSchema eventSchema)
		{
            ClrAssemblyName = ev.GetType().Assembly.FullName;
            ClrTypeName = ev.GetType().FullName;
            EventJson = JsonConvert.SerializeObject(ev);
            EventJsonSchema = eventSchema.ToString();
		}

		public string ClrAssemblyName { get; private set; }

		public string ClrTypeName { get; private set; }

		public string EventJson { get; private set; }

		public string EventJsonSchema { get; private set; }
	}
}
