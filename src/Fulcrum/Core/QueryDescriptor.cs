using System.Reflection;

namespace Fulcrum.Core
{
	// TODO: clarify the ambiguity query methods not being directly represented in IQuery
	public class QueryDescriptor
	{
		public QueryDescriptor(string queryObject, string @namespace, MethodInfo methodInfo)
		{
			Namespace = @namespace;
			QueryObject = queryObject;
			Query = methodInfo.Name;
		}

		public string Query { get; private set; }

		public string QueryObject { get; private set; }

		public string Namespace { get; private set; }
	}
}
