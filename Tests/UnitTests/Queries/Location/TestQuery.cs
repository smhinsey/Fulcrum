using System;
using Fulcrum.Core;

namespace UnitTests.Queries.Location
{
	public class TestQuery : IQuery
	{
		public int GetAnInt()
		{
			return new Random().Next();
		}
	}
}