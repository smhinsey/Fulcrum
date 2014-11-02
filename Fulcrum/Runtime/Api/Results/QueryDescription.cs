using System;
using System.Collections.Generic;
using Fulcrum.Core;

namespace Fulcrum.Runtime.Api.Results
{
	public class QueryDescription : QueryDescriptor
	{
		public QueryDescription(string parentNamespace, string name, string method,
			IDictionary<string, Type> parameters) :
				base(parentNamespace, name, method, parameters)
		{
		}
	}
}
