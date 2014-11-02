using System;
using System.Collections.Generic;

namespace Fulcrum.Core
{
	public class QueryDescriptor
	{
		public QueryDescriptor(string name, string @namespace, string method, IDictionary<string, Type> parameters)
		{
			Parameters = parameters;
			Method = method;
			Namespace = @namespace;
			Name = name;
		}

		public string Name { get; private set; }

		public string Namespace { get; private set; }

		public string Method { get; private set; }

		public IDictionary<string, Type> Parameters { get; private set; } 
	}
}
