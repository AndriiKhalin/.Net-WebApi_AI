namespace LB3.Features;

public class PaginateViewModel<T> : List<T>
{

    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public int PageCount { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<T> Items { get; set; }
    public PaginateViewModel(IEnumerable<T> items, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageCount = items.Count();
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(PageCount / (double)pageSize);
        Items = items;
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public async Task<List<T>> CreateAsync()
    {
        return Items.Skip(
                (PageIndex - 1) * PageSize)
            .Take(PageSize).ToList();
    }
}