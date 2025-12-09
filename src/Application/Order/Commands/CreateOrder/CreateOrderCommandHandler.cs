using Application.Basket.Queries.GetCartList;
using Application.Catalog.Products.Queries.GetVariantById;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObject;
using MediatR;

namespace Application.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMediator _mediator;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IMediator mediator)
    {
        _orderRepository = orderRepository;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
#if (!GrpcOrHttp)
        // Directly call the mediatr handler instead of gRPC
        var basket = await _mediator.Send(new GetBasketQuery(command.CustomerId))
            ?? throw new Exception("Basket not found");
#endif

        if (!basket.Items.Any())
            throw new Exception("Basket is empty. Cannot create order.");

        var orderItems = new List<OrderItem>();
        //decimal totalAmount = 0;
        Money totalAmount = Money.InitValue();

        foreach (var basketItem in basket.Items)
        {
#if (!GrpcOrHttp)
            // Directly call the mediatr handler instead of gRPC
            var variant = await _mediator.Send(new GetVariantByIdQuery(basketItem.ProductVariantId));
#endif
            if (variant == null)
            {
                throw new EntityNotFoundException();
            }

            if (variant.Quantity < basketItem.Quantity)
            {
                throw new Exception("Not enough product variant quantity");
            }

            var image = variant.Image.Url ?? string.Empty;

            var orderItem = new OrderItem
            {
                VariantId = variant.Id,
                ProductName = variant.ProductName,
                VariantTitle = variant.Title,
                UnitPrice = variant.Price,
                Quantity = basketItem.Quantity,
                ImageUrl = image
            };

            orderItems.Add(orderItem);
            //totalAmount += orderItem.TotalPrice;
            totalAmount += Money.Vnd(orderItem.TotalPrice);
        }

        var order = Domain.Entities.Order.Create(
            Guid.NewGuid(),
            DateTime.Now.Ticks,
            command.CustomerId,
            Address.Of(command.Street, command.City, command.ZipCode),
            orderItems,
            totalAmount);

        await _orderRepository.AddAsync(order);
        await _orderRepository.UnitOfWork.SaveChangesAsync();

        return order.Id;
    }
}
