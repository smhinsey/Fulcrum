using System;
using System.Linq;
using Fulcrum.Core;

namespace Fulcrum.Runtime
{
	[AttributeUsage(AttributeTargets.Class)]
	public class QueryValidationAttribute : Attribute
	{
		public QueryValidationAttribute(Type queryType)
		{
			if (queryType.GetInterfaces().Any(x =>
				x.IsGenericType &&
				x.GetGenericTypeDefinition() != typeof(ICommandValidationQuery<>)))
			{
				throw new ArgumentException
					("You must supply an type which implements ICommandValidationQuery", "queryType");
			}

			Descriptor = new QueryDescriptor(queryType.Name, queryType.Namespace);
		}

		public QueryDescriptor Descriptor { get; private set; }
	}
}
