namespace Fulcrum.Common
{
	public interface IPagedList
	{
		int CurrentPage { get; }

		int PageSize { get; }

		int Skip { get; }

		int TotalResults { get; }

		int Count { get; }
	}
}
