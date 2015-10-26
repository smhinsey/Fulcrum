using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fulcrum.Runtime
{
	public interface ICommandLocator
	{
		void AddCommandSource(Assembly assembly, string inNamespace);

		Type Find(string commandName, string inNamespace);

		/// <summary>
		///   Returns all instances of ICommand located in the configured assembly and
		///   namespace.
		/// </summary>
		/// <returns>A list of command types in the specified location.</returns>
		IList<Type> ListAllCommands();
	}
}
