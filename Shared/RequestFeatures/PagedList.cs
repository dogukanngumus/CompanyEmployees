namespace Shared.RequestFeatures;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; set; }
    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData()
        {
            CurrentPage = pageNumber,
            PageSize = pageSize, 
            TotalCount = count, 
            TotalPages =  (int)Math.Ceiling(count / (double)pageSize)
        };
        AddRange(items);
    }

    public static PagedList<T> ToPagedList(IEnumerable<T> items, int pageNumber, int pageSize)
    {
        var pagedItems = items.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
        int count = items.Count();
        return new PagedList<T>(pagedItems, count, pageNumber, pageSize);
    }
    
}
