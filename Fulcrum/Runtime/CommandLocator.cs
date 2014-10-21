using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using Fulcrum.Core;

namespace Fulcrum.Runtime
{
	public class CommandLocator : ICommandLocator
	{
		private readonly IDictionary<string, Assembly> _assemblies;

		private readonly IList<Type> _commands;

		public CommandLocator()
		{
			_assemblies = new Dictionary<string, Assembly>();
			_commands = new List<Type>();
		}

		public CommandLocator(Assembly assembly, string inNamespace) : this()
		{
			AddCommandSource(assembly, inNamespace);
		}

		public void AddCommandSource(Assembly assembly, string inNamespace)
		{
			if (_assemblies.ContainsKey(inNamespace))
			{
				return;
			}

			_assemblies.Add(inNamespace, assembly);

			assembly.GetTypes()
				.Where(type => typeof(ICommand).IsAssignableFrom(type)
				               && type.Namespace == inNamespace)
				.ForEach(_commands.Add);
		}

		public Type FindInNamespace(string commandName, string inNamespace)
		{
			return _commands.FirstOrDefault(t => t.Name == commandName && t.Namespace == inNamespace);
		}

		/// <summary>
		///   Returns all instances of ICommand located in the configured assembly and
		///   namespace.
		/// </summary>
		/// <returns>A list of command types in the specified location.</returns>
		public IList<Type> ListAllCommands()
		{
			var types = new List<Type>();

			foreach (var assembly in _assemblies)
			{
				types.AddRange(assembly.Value.GetTypes()
					.Where(type => typeof(ICommand).IsAssignableFrom(type)
					               && type.Namespace == assembly.Key));
			}

			return types;
		}
	}
}
