namespace Api.Models.Responses;

public class PaginatedResult<TEntity>(int pageIndex, int pageSize, int count, int pageList, IEnumerable<TEntity> items) where TEntity : class
{
    public int PageIndex => pageIndex;
    public int PageSize => pageSize;
    public int Count => count;
    public int PageList => pageList;
    public IEnumerable<TEntity> Items => items;
}
