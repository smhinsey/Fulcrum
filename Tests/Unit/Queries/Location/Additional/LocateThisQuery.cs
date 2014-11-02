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

		public IList<string> GetSomeText(string userName, bool lotsOfStrings)
		{
			var howMany = 10;

			if (lotsOfStrings)
			{
				howMany = 50;
			}

			var rng = new Random();

			var result = new List<string>();

			const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz .,!?";

			foreach (var text in Enumerable.Range(0, howMany))
			{
				var randomText = new char[rng.Next(5, 45)];

				for (var i = 0; i < randomText.Length; i++)
				{
					randomText[i] = Characters[rng.Next(Characters.Length)];
				}

				result.Add(new String(randomText));
			}

			return result;
		}
	}
}
