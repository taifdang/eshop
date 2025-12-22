namespace Api.Models.Requests;

public record CreateOrderRequestDto(Guid CustomerId, string Street, string City, string ZipCode);
