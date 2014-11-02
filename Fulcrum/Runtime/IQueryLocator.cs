using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fulcrum.Runtime
{
	public interface IQueryLocator
	{
		void AddQuerySource(Assembly assembly, string inNamespace);

		Type FindInNamespace(string queryName, string inNamespace);

		IList<Type> ListAllQueryGroups();

		IList<MethodInfo> ListQueriesInQueryObject(Type groupType);
	}
}
