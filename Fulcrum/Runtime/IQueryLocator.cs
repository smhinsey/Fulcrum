using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fulcrum.Runtime
{
	public interface IQueryLocator
	{
		void AddQuerySource(Assembly assembly, string inNamespace);

		Type Find(string queryName, string inNamespace);

		IList<Type> ListAllQueryObjects();

		IList<MethodInfo> ListQueriesInQueryObject(Type groupType);
	}
}
