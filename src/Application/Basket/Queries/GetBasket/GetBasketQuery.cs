using Application.Basket.Queries.GetBasket;
using Application.Basket.Specifications;
using Application.Catalog.Products.Queries.GetVariantById;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Customer.Queries.GetCustomerByUserId;
using MediatR;
using Shared.Web;

namespace Application.Basket.Queries.GetCartList;

public record GetBasketQuery(Guid? CustomerId = null) : IRequest<BasketDto>;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, BasketDto>
{
    private readonly IRepository<Domain.Entities.Basket> _basketRepo;
    private readonly IMediator _mediator;
    private readonly ICurrentUserProdvider _currentUserProdvider;
    public GetBasketQueryHandler(
        IMediator mediator,
        ICurrentUserProdvider currentUserProdvider,
        IRepository<Domain.Entities.Basket> basketRepo)
    {
        _mediator = mediator;
        _currentUserProdvider = currentUserProdvider;
        _basketRepo = basketRepo;
    }
    public async Task<BasketDto> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        Guid? customerId;

        if (!request.CustomerId.HasValue)
        {
            var userId = _currentUserProdvider.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) 
                throw new EntityNotFoundException("User not found");

            // Directly call the GetCustomerByUserIdQuery handler instead of gRPC
            var customer = await _mediator.Send(new GetCustomerByUserIdQuery(Guid.Parse(userId)), cancellationToken)
                ?? throw new EntityNotFoundException("Customer not found");
            customerId = customer.Id;
        }
        else
        {
            customerId = request.CustomerId.Value;
        }

        if (customerId is null) 
            throw new EntityNotFoundException("User not found");

        // Get basket
        var spec = new BasketSpec()
            .ByCustomerId(customerId.Value)
            .WithItems();
        
        var basket = await _basketRepo.FirstOrDefaultAsync(spec, cancellationToken);

        // If basket doesn't exist, return an empty basket
        if (basket == null)
        {
            return new BasketDto(Guid.NewGuid(), customerId.Value, new List<BasketItemDto>(), DateTime.UtcNow, null);
        }

        BasketDto basketDto = new BasketDto(basket.Id, customerId.Value, new List<BasketItemDto>(), (DateTime)basket.CreatedAt, basket.LastModified);

        foreach (var item in basket.Items)
        {
            var productVariant = await _mediator.Send(new GetVariantByIdQuery(item.ProductVariantId));
            var cartItem = basket.Items.First(x => x.ProductVariantId == productVariant.Id);
            basketDto.Items.Add(new BasketItemDto(cartItem.ProductVariantId, productVariant.ProductName ,productVariant.Title, productVariant.Price, productVariant.Image.Url ?? "", cartItem.Quantity));
        }

        return basketDto;
    }
}
