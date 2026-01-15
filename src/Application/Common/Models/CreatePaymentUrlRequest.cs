
namespace Application.Common.Models;

public class CreatePaymentUrlRequest
{
    public long OrderNumber { get; init; }
    public decimal Amount { get; init; }
    public DateTime OrderDate { get; init; }
}
