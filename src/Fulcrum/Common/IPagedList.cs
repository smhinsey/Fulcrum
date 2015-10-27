namespace Fulcrum.Common
{
	public interface IPagedList
	{
		int Count { get; }

		int CurrentPage { get; }

		int PageSize { get; }

		int Skip { get; }

		int TotalResults { get; }
	}
}
