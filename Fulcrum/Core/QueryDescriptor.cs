using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fulcrum.Core
{
	public class QueryDescriptor
	{
		public QueryDescriptor(string name, string @namespace, MethodInfo methodInfo)
		{
			Namespace = @namespace;
			Name = name;
		}

		public string Name { get; private set; }

		public string Namespace { get; private set; }

		public string Method { get; private set; }

		public IDictionary<string, Type> Parameters { get; private set; } 
	}
}
