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
			if (queryType.GetInterfaces()
				.Any(x =>
					x.IsGenericType &&
					x.GetGenericTypeDefinition() != typeof(ICommandValidationQuery<>)))
			{
				throw new ArgumentException
					("You must supply a type which implements ICommandValidationQuery", "queryType");
			}

			var method = queryType.GetMethod("ValidateCommand");

			Descriptor = new QueryDescriptor(queryType.Name, queryType.Namespace, method);
		}

		public QueryDescriptor Descriptor { get; private set; }
	}
}
