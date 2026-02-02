using Application.Order.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Order.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid CustomerId,
    PaymentMethod Method, 
    PaymentProvider Provider,
    string Street,
    string City, 
    string ZipCode) : IRequest<CreateOrderResult>;
