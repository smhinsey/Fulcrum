using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using Fulcrum.Core;

namespace Fulcrum.Runtime
{
	public class EventLocator : IEventLocator
	{
		private readonly IDictionary<string, Assembly> _assemblies;

		private readonly IList<Type> _events;

		public EventLocator()
		{
			_assemblies = new Dictionary<string, Assembly>();
			_events = new List<Type>();
		}

        public EventLocator(Assembly assembly, string inNamespace)
            : this()
		{
			AddEventSource(assembly, inNamespace);
		}

		public void AddEventSource(Assembly assembly, string inNamespace)
		{
			if (_assemblies.ContainsKey(inNamespace))
			{
				return;
			}

			_assemblies.Add(inNamespace, assembly);

			assembly.GetTypes()
				.Where(type => typeof(IEvent).IsAssignableFrom(type)
				               && type.Namespace == inNamespace)
				.ForEach(_events.Add);
		}

		public Type FindInNamespace(string eventName, string inNamespace)
		{
			return _events.FirstOrDefault(t => t.Name == eventName && t.Namespace == inNamespace);
		}

		/// <summary>
		///   Returns all instances of IEvent located in the configured assembly and
		///   namespace.
		/// </summary>
		/// <returns>A list of event types in the specified location.</returns>
		public IList<Type> ListAllEvents()
		{
			var types = new List<Type>();

			foreach (var assembly in _assemblies)
			{
				types.AddRange(assembly.Value.GetTypes()
					.Where(type => typeof(IEvent).IsAssignableFrom(type)
					               && type.Namespace == assembly.Key));
			}

			return types;
		}
	}
}
