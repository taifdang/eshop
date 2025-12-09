namespace Contracts.Requests;

public class PaginationRequest(int pageSize = 10, int pageIndex = 0)
{
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
}
