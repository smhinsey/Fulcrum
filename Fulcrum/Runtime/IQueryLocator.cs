using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fulcrum.Runtime
{
	public interface IQueryLocator
	{
		void AddQuerySource(Assembly assembly, string inNamespace);

		Type FindInNamespace(string commandName, string inNamespace);

		/// <summary>
		///   Returns all instances of IQuery located in the configured assembly and
		///   namespace.
		/// </summary>
		/// <returns>A list of query types in the specified location.</returns>
		IList<Type> ListAllQueries();
 
	}
}