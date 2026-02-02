using Application.Abstractions;
using Application.Basket.Dtos;
using Application.Catalog.Products.Queries.GetVariantById;
using Application.Common.Exceptions;
using Application.Common.Services;
using Application.Customer.Queries.GetCustomerByUserId;
using Domain.Repositories;
using MediatR;

namespace Application.Basket.Queries.GetCartList;

public record GetBasketQueryByCustomer(Guid? CustomerId = null) : IRequest<BasketDto>;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQueryByCustomer, BasketDto>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProdvider;

    public GetBasketQueryHandler(
        IBasketRepository basketRepository,
        IMediator mediator,
        ICurrentUserProvider currentUserProdvider,
        ICrudService<Domain.Entities.Basket> basketService)
    {
        _basketRepository = basketRepository;
        _mediator = mediator;
        _currentUserProdvider = currentUserProdvider;
    }

    public async Task<BasketDto> Handle(GetBasketQueryByCustomer request, CancellationToken cancellationToken)
    {
        Guid? customerId;

        if (!request.CustomerId.HasValue)
        {
            var userId = _currentUserProdvider.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) 
                throw new EntityNotFoundException("User not found");

#if (!GrpcOrHttp)
            // Directly call the mediatr handler instead of gRPC / http
            var customer = await _mediator.Send(new GetCustomerByUserIdQuery(Guid.Parse(userId)), cancellationToken)
               ?? throw new EntityNotFoundException("Customer not found");
            customerId = customer.Id;
#endif
        }
        else
        {
            customerId = request.CustomerId.Value;
        }

        if (customerId is null) 
            throw new EntityNotFoundException("User not found");

        var basket = await _basketRepository.GetByCustomerIdWithItemsAsync(customerId.Value);

        // If basket doesn't exist, return an empty basket
        if (basket == null)
        {
            return new BasketDto(Guid.NewGuid(), customerId.Value, new List<BasketItemDto>(), DateTime.UtcNow, null);
        }

        BasketDto basketDto = new BasketDto(basket.Id, customerId.Value, new List<BasketItemDto>(), (DateTime)basket.CreatedAt, basket.UpdatedAt);
        //sort items by VariantId to ensure consistent order
        var items = basket.Items
            .OrderBy(x => x.VariantId)
            .ToList();

        foreach (var item in items)
        {
#if (!GrpcOrHttp)
            // Directly call the mediatr handler instead of gRPC / http
            var variant = await _mediator.Send(new GetVariantByIdQuery(item.VariantId));
#endif
            basketDto.Items.Add(new BasketItemDto(item.VariantId, variant.ProductName, variant.Title, variant.Price, variant.Image.Url ?? string.Empty, item.Quantity));
        }

        return basketDto;
    }
}
