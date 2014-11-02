using System;
using System.Collections.Generic;
using System.Linq;
using Fulcrum.Core;

namespace Tests.Unit.Queries.Location.Additional
{
	public class LocateThisQuery : IQuery
	{
		public IList<int> GetSomeNumbers(string userName, bool lotsOfNumbers)
		{
			var howMany = 10;

			if (lotsOfNumbers)
			{
				howMany = 50;
			}

			var rng = new Random();

			return Enumerable.Range(0, howMany).Select(number => rng.Next()).ToList();
		}
	}
}
