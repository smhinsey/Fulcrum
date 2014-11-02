using System.Reflection;
using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results
{
	public class QueryDescription : QueryDescriptor
	{
		public QueryDescription(string name, string @namespace, MethodInfo methodInfo)
			: base(name, @namespace, methodInfo)
		{
		}
	}
}
