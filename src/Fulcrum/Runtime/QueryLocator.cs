using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using Fulcrum.Common;
using Fulcrum.Core;

namespace Fulcrum.Runtime
{
	public class QueryLocator : IQueryLocator, ILoggingSource
	{
		private readonly IDictionary<string, Assembly> _assemblies;

		private readonly IList<Type> _queries;

		public QueryLocator()
		{
			_assemblies = new Dictionary<string, Assembly>();
			_queries = new List<Type>();
		}

		public QueryLocator(Assembly assembly, string inNamespace)
			: this()
		{
			AddQuerySource(assembly, inNamespace);
		}

		public void AddQuerySource(Assembly assembly, string inNamespace)
		{
			if (_assemblies.ContainsKey(inNamespace))
			{
				return;
			}

			_assemblies.Add(inNamespace, assembly);

			assembly.GetTypes()
				.Where(type => typeof(IQuery).IsAssignableFrom(type)
				               && type.Namespace == inNamespace)
				.ForEach(_queries.Add);
		}

		public Type Find(string queryName, string inNamespace)
		{
			var matchCount = _queries.Count(q => q.Name == queryName);

			if (matchCount == 1)
			{
				return _queries.FirstOrDefault(t => t.Name == queryName);
			}
			
			if (matchCount > 1)
			{
				this.LogInfo("More than one query object found for {0}. Narrowing by namespace {1}.",
					queryName, inNamespace);

				return _queries.FirstOrDefault(t => t.Name == queryName
																												 && t.Namespace == inNamespace);
			}
			
			var message = string.Format("Query {0} in {1} matched {2} registered types.",
				queryName, inNamespace, matchCount);

			this.LogWarn(message);

			throw new Exception(message);
		}

		public IList<Type> ListAllQueryObjects()
		{
			var types = new List<Type>();

			foreach (var assembly in _assemblies)
			{
				types.AddRange(assembly.Value.GetTypes()
					.Where(type => typeof(IQuery).IsAssignableFrom(type)
					               && type.Namespace == assembly.Key));
			}

			return types;
		}

		public IList<MethodInfo> ListQueriesInQueryObject(Type queryGroup)
		{
			if (queryGroup == null)
			{
				throw new ArgumentNullException("queryGroup");
			}

			var results = new List<MethodInfo>();

			var methods = queryGroup.GetMethods();

			foreach (var method in methods)
			{
				if (method.DeclaringType == queryGroup && method.IsPublic)
				{
					results.Add(method);
				}
			}

			return results;
		}
	}
}
