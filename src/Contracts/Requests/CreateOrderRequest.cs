namespace Contracts.Requests;

public record CreateOrderRequest(Guid CustomerId, string Street, string City, string ZipCode);