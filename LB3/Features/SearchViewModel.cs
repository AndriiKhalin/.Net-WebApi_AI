namespace LB3.Features;

public class SearchViewModel<T> where T : IEntity
{
    public SearchViewModel()
    {

    }

    public static IEnumerable<T> Search(IEnumerable<T> list, string searchString)
    {
        searchString = searchString.Replace(" ", "").Trim();
        return list.Where(x => x.Name.Replace(" ", "").Trim().Contains(searchString));
    }
}