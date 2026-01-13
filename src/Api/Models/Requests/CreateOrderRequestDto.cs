using Domain.Enums;

namespace Api.Models.Requests;

public record CreateOrderRequestDto(
    Guid CustomerId,
    PaymentMethod Method,
    PaymentProvider Provider,
    string Street, 
    string City, 
    string ZipCode);
