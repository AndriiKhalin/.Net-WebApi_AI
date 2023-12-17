using System.Diagnostics.Metrics;

namespace LB3.Features;

public class SortViewModel<T> where T : IEntity
{
    public SortState NameSort { get; }
    public SortState IdSort { get; }
    public SortState PriceSort { get; }

    public SortState Current { get; }

    public string Up_DownId { get; }
    public string Up_DownName { get; }
    public string Up_DownPrice { get; }
    public SortViewModel(SortState sortOrder)
    {
        IdSort = sortOrder == SortState.IdAsc ? SortState.IdDesc :
            sortOrder == SortState.IdDesc ? SortState.IdAsc :
            SortState.IdDesc;
        NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
        PriceSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;


        Up_DownId = sortOrder == SortState.IdAsc ? "fa fa-angle-up" :
            sortOrder == SortState.IdDesc ? "fa fa-angle-down" :
            "";
        Up_DownName = sortOrder == SortState.NameAsc ? "fa fa-angle-up" :
            sortOrder == SortState.NameDesc ? "fa fa-angle-down" :
            "";
        Up_DownPrice = sortOrder == SortState.PriceAsc ? "fa fa-angle-up" :
            sortOrder == SortState.PriceDesc ? "fa fa-angle-down" :
            "";
        Current = sortOrder;
    }

    public IEnumerable<T> SortList(IEnumerable<T> lists, SortState sortOrder)
    {
        return sortOrder switch
        {
            SortState.IdDesc => lists.OrderByDescending(s => s.Id).ToList(),
            SortState.NameAsc => lists.OrderBy(s => s.Name).ToList(),
            SortState.NameDesc => lists.OrderByDescending(s => s.Name).ToList(),
            SortState.PriceAsc => lists.OrderBy(s => s.Price).ToList(),
            SortState.PriceDesc => lists.OrderByDescending(s => s.Price).ToList(),
            _ => lists.OrderBy(s => s.Id).ToList(),
        };
    }
}

