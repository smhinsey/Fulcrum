using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fulcrum.Core;

namespace Fulcrum.Runtime
{
	public class CommandLocator
	{
		/// <summary>
		/// Returns a list of types located in assembly in the namespace inNamespace which
		/// implement ICommand.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="inNamespace"></param>
		/// <returns>A list of command types in the specified location.</returns>
		public IList<Type> FindCommands(Assembly assembly, string inNamespace)
		{
			var types = assembly.GetTypes()
				.Where(type => typeof(ICommand).IsAssignableFrom(type)
				               && type.Namespace == inNamespace);

			return types.ToList();
		}
	}
}
