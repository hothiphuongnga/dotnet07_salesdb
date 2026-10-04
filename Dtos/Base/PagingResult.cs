namespace SalesDB.Dtos.Base;

public class PagingResult<T>
{
    // items : ds T
    // size, page, keyworks, totalRow
     public List<T> Items { get; set; } = new();

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int TotalItems { get; set; }

    public int TotalPages { get; set; }
}