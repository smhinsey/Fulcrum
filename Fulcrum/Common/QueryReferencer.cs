using System;
using System.Linq.Expressions;

namespace Fulcrum.Common
{
	public static class QueryReferencer
	{
		public static string GetName<T>(Expression<Func<T, object>> expression)
		{
			if (expression == null)
			{
				throw new ArgumentException("The expression cannot be null.");
			}

			var methodExpression = expression.Body as MethodCallExpression;

			if (methodExpression == null)
			{
				throw new ArgumentException("expression must be a method");
			}

			var methodCallExpression = methodExpression;

			// TODO: can we convert the calling expression into a fully-executable query URL?

			//var args = new List<string>();

			//foreach (var parameter in methodCallExpression.Arguments)
			//{
			//	args.Add(string.Format("{0}?{1}", parameter));
			//}

			return string.Format("{0}/{1}/{2}",
				methodCallExpression.Method.DeclaringType.Namespace,
				methodCallExpression.Method.DeclaringType.Name,
				methodCallExpression.Method.Name);
		}
	}
}
