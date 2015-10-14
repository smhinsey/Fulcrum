using System;
using System.Collections.Generic;

namespace Fulcrum.Common
{
	public class ResultPage<T> : List<T>, IPagedList
	{
		public ResultPage(int totalResults, int pageSize, int skip, IEnumerable<T> collection)
			: base(collection)
		{
			Skip = skip;
			PageSize = pageSize;
			TotalResults = totalResults;

			CurrentPage = (int)Math.Ceiling((skip + 1) /(decimal)PageSize);
		}

		public int CurrentPage { get; private set; }

		public int PageSize { get; private set; }

		public int Skip { get; private set; }

		public int TotalResults { get; private set; }
	}
}
