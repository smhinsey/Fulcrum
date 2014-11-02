using System;
using Fulcrum.Core;

namespace Tests.Unit.Queries
{
	public class TestQuery : IQuery
	{
		public int GetAnInt()
		{
			return new Random().Next();
		}
	}
}