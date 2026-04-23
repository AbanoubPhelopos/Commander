namespace commander.domain.Common;

public class PaginatedList<T>
{
    public IReadOnlyList<T> Items { get; }

    public int PageIndex { get; }
    public int PageSize { get; }

    public int TotalCount { get; }

    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public PaginatedList()
    {
        Items = [];
    }

    public PaginatedList(IReadOnlyList<T> items, int pageIndex, int pageSize, int totalCount)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
