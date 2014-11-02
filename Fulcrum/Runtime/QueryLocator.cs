using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using Fulcrum.Core;

namespace Fulcrum.Runtime
{
	public class QueryLocator : IQueryLocator
	{
		private readonly IDictionary<string, Assembly> _assemblies;

		private readonly IList<Type> _commands;

		public QueryLocator()
		{
			_assemblies = new Dictionary<string, Assembly>();
			_commands = new List<Type>();
		}

		public QueryLocator(Assembly assembly, string inNamespace)
			: this()
		{
			AddQuerySource(assembly, inNamespace);
		}

		public void AddQuerySource(Assembly assembly, string inNamespace)
		{
			if (_assemblies.ContainsKey(inNamespace))
			{
				return;
			}

			_assemblies.Add(inNamespace, assembly);

			assembly.GetTypes()
				.Where(type => typeof(IQuery).IsAssignableFrom(type)
				               && type.Namespace == inNamespace)
				.ForEach(_commands.Add);
		}

		public Type FindInNamespace(string commandName, string inNamespace)
		{
			return _commands.FirstOrDefault(t => t.Name == commandName && t.Namespace == inNamespace);
		}

		public IList<Type> ListAllQueries()
		{
			var types = new List<Type>();

			foreach (var assembly in _assemblies)
			{
				types.AddRange(assembly.Value.GetTypes()
					.Where(type => typeof(IQuery).IsAssignableFrom(type)
					               && type.Namespace == assembly.Key));
			}

			return types;
		}
	}
}
