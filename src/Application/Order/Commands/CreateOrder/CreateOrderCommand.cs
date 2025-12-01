using Application.Basket.Queries.GetCartList;
using Application.Catalog.Products.Queries.GetVariantById;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObject;
using MediatR;

namespace Application.Order.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid CustomerId, 
    Address ShippingAddress,
    PaymentMethod PaymentMethod,
    PaymentProvider? PaymentProvider = null,
    string? PaymentUrl = null) : IRequest<Guid>;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IRepository<Domain.Entities.Order> _orderRepo;
    private readonly IMediator _mediator;

    public CreateOrderCommandHandler(
        IRepository<Domain.Entities.Order> orderRepo,
        IMediator mediator)
    {
        _orderRepo = orderRepo;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var basket = await _mediator.Send(new GetBasketQuery(request.CustomerId))
            ?? throw new Exception("Basket not found");

        if (!basket.Items.Any())
            throw new Exception("Basket is empty. Cannot create order.");

        var orderItems = new List<OrderItem>();
        //decimal totalAmount = 0;
        Money totalAmount = Money.InitValue();

        foreach (var basketItem in basket.Items)
        {
            var productVariant = await _mediator.Send(new GetVariantByIdQuery(basketItem.ProductVariantId));

            if (productVariant == null)
            {
                throw new EntityNotFoundException();
            }

            if (productVariant.Quantity < basketItem.Quantity)
            {
                throw new Exception("Not enough product variant quantity");
            }

            var orderItem = new OrderItem
            {
                ProductVariantId = productVariant.Id,     
                ProductName = productVariant.ProductName,
                VariantName = productVariant.Title,
                UnitPrice = productVariant.Price,
                Quantity = basketItem.Quantity,
                ImageUrl = productVariant.Image.Url
            };

            orderItems.Add(orderItem);
            //totalAmount += orderItem.TotalPrice;
            totalAmount += Money.Vnd(orderItem.TotalPrice);
        }

        Payment payment = request.PaymentMethod switch
        {
            PaymentMethod.Cod => Payment.CreateCod(totalAmount),
            PaymentMethod.Online => Payment.CreateOnline(request.PaymentProvider.Value, totalAmount, request.PaymentUrl),
            _ => throw new Exception("Payment method not support")
        };

        var order = Domain.Entities.Order.Create(
            Guid.NewGuid(),
            request.CustomerId,
            request.ShippingAddress, 
            orderItems, 
            totalAmount,
            payment);

        await _orderRepo.AddAsync(order);

        return order.Id;
    }
}

// event
//var orderItemsAdded = _mapper.Map<List<OrderItemDto>>(orderItems);

// Update product variant quanity
// await _mediator.Send(new ReduceStockCommand(orderItemsAdded));

// Clear the basket after creating the order
//await _mediator.Send(new Basket.Commands.ClearBasket.ClearBasketCommand(request.CustomerId));
