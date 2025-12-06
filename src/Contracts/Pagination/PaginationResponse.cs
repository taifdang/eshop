namespace Contracts.Pagination;

public class PaginatedResult<TEntity>(int  pageIndex, int pageSize, int count, IEnumerable<TEntity> items) where TEntity : class
{
    public int PageIndex => pageIndex;
    public int PageSize => pageSize;
    public int Count => count;
    public IEnumerable<TEntity> Items => items;
}
