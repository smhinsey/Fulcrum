using System;
using System.Collections.Generic;
using Fulcrum.Runtime;
using Tests.Unit.Queries.Location.Additional;
using Xunit;

namespace Tests.Unit.Queries
{
	public class QueryLocatorTests
	{
		[Fact]
		public void Find_query_in_namespace()
		{
			var queryType = typeof(TestQuery);

			var extractor = new QueryLocator();

			extractor.AddQuerySource(queryType.Assembly, queryType.Namespace);

			var locatedType = extractor.FindInNamespace(queryType.Name, queryType.Namespace);

			Assert.Equal(queryType, locatedType);
		}

		[Fact]
		public void List_all_queries_in_assembly_via_multiple_namespaces()
		{
			var firstQueryType = typeof(TestQuery);
			var secondQueryType = typeof(LocateThisQuery);

			var extractor = new QueryLocator(firstQueryType.Assembly, firstQueryType.Namespace);

			extractor.AddQuerySource(secondQueryType.Assembly, secondQueryType.Namespace);

			var queries = extractor.ListAllQueryGroups();

			var expectedQueries = new List<Type>()
			{
				typeof(TestQuery),
				typeof(LocateThisQuery),
			};

			Assert.Equal(expectedQueries, queries);
		}

		[Fact]
		public void List_all_queries_in_assembly_via_namespace_and_constructor()
		{
			var queryType = typeof(LocateThisQuery);

			var extractor = new QueryLocator(queryType.Assembly, queryType.Namespace);

			var queries = extractor.ListAllQueryGroups();

			var expectedQueries = new List<Type>()
			{
				typeof(LocateThisQuery),
			};

			Assert.Equal(expectedQueries, queries);
		}

		[Fact]
		public void List_all_queries_in_assembly_via_namespace_and_method()
		{
			var queryType = typeof(LocateThisQuery);

			var extractor = new QueryLocator();

			extractor.AddQuerySource(queryType.Assembly, queryType.Namespace);

			var commands = extractor.ListAllQueryGroups();

			var expectedCommands = new List<Type>()
			{
				typeof(LocateThisQuery),
			};

			Assert.Equal(expectedCommands, commands);
		}
	}
}
