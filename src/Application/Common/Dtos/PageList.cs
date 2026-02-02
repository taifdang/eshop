namespace Application.Common.Dtos;

public class PageList<TEntity>(IEnumerable<TEntity> items, int count, int pageIndex, int pageSize) where TEntity : class
{
    public int PageIndex => pageIndex;
    public int PageSize => pageSize;
    public int Count => count;
    public IEnumerable<TEntity> Items => items;
    //public int TotalPage => (int)Math.Ceiling(count / (double)pageSize);
}