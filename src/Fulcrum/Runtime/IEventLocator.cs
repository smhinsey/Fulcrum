using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fulcrum.Runtime
{
	public interface IEventLocator
	{
		void AddEventSource(Assembly assembly, string inNamespace);

		Type FindInNamespace(string eventName, string inNamespace);

		/// <summary>
		///   Returns all instances of IEvent located in the configured assembly and
		///   namespace.
		/// </summary>
		/// <returns>A list of event types in the specified location.</returns>
		IList<Type> ListAllEvents();
	}
}
