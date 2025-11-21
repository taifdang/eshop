using Application.Basket.Queries.GetCartList;
using Application.Catalog.Variants.Queries.GetVariantById;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Order.Commands.CreateOrder;

public record CreateOrderCommand(Guid CustomerId, string ShippingAddress) : IRequest<Guid>;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IRepository<Domain.Entities.Order> _orderRepo;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public CreateOrderCommandHandler(
        IRepository<Domain.Entities.Order> orderRepo,
        IMediator mediator,
        IMapper mapper)
    {
        _orderRepo = orderRepo;
        _mediator = mediator;
        _mapper = mapper;
    }
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var basket = await _mediator.Send(new GetBasketQuery(request.CustomerId));

        if(basket?.Items == null || !basket.Items.Any())
        {
            throw new Exception("Basket is empty. Cannot create order.");
        }

        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        foreach (var basketItem in basket.Items)
        {
            var productVariant = await _mediator.Send(new GetVariantByIdQuery(basketItem.ProductVariantId));

            if (productVariant == null)
            {
                throw new EntityNotFoundException();
            }

            if(productVariant.Quantity < basketItem.Quantity)
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
            totalAmount += orderItem.TotalPrice;
        }

        //var order = new Domain.Entities.Order
        //{
        //    CustomerId = request.CustomerId,
        //    Status = OrderStatus.Pending,
        //    TotalAmount = totalAmount,
        //    ShippingAddress = request.ShippingAddress,
        //    Items = orderItems,
        //    OrderDate = DateTime.UtcNow,
        //    CreatedAt = DateTime.UtcNow
        //};
        var order = Domain.Entities.Order.Create(Guid.NewGuid(), request.CustomerId,
                request.ShippingAddress, orderItems, totalAmount);

        await _orderRepo.AddAsync(order);

        // event
        //var orderItemsAdded = _mapper.Map<List<OrderItemDto>>(orderItems);

        // Update product variant quanity
        // await _mediator.Send(new ReduceStockCommand(orderItemsAdded));

        // Clear the basket after creating the order
        //await _mediator.Send(new Basket.Commands.ClearBasket.ClearBasketCommand(request.CustomerId));

        return order.Id;
    }
}
