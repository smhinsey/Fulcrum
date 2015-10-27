using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using Fulcrum.Common;
using Fulcrum.Core;

namespace Fulcrum.Runtime
{
	public class CommandLocator : ICommandLocator, ILoggingSource
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

		public Type Find(string commandName, string inNamespace)
		{
			var matchCount = _commands.Count(q => q.Name == commandName);

			if (matchCount == 1)
			{
				return _commands.FirstOrDefault(t => t.Name == commandName);
			}

			if (matchCount > 1)
			{
				this.LogInfo("More than one command found for {0}. Narrowing by namespace {1}.",
					commandName, inNamespace);

				return _commands.FirstOrDefault(t => t.Name == commandName
				                                     && t.Namespace == inNamespace);
			}

			var message = string.Format("Command {0} in {1} matched {2} registered types.",
				commandName, inNamespace, matchCount);

			this.LogWarn(message);

			throw new Exception(message);
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
