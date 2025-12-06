using Application.Basket.Queries.GetBasket;
using Application.Catalog.Products.Queries.GetVariantById;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Customer.Queries.GetCustomerByUserId;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Web;

namespace Application.Basket.Queries.GetCartList;

public record GetBasketQuery(Guid? CustomerId = null) : IRequest<BasketDto>;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, BasketDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly ICurrentUserProdvider _currentUserProdvider;
    public GetBasketQueryHandler(
        IMediator mediator,
        ICurrentUserProdvider currentUserProdvider,
        IApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _currentUserProdvider = currentUserProdvider;
        _dbContext = dbContext;
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
        var basket = await _dbContext.Baskets
            .Include(m => m.Items)
            .FirstOrDefaultAsync(b => b.CustomerId == customerId.Value);

        // If basket doesn't exist, return an empty basket
        if (basket == null)
        {
            return new BasketDto(Guid.NewGuid(), customerId.Value, new List<BasketItemDto>(), DateTime.UtcNow, null);
        }

        BasketDto basketDto = new BasketDto(basket.Id, customerId.Value, new List<BasketItemDto>(), (DateTime)basket.CreatedAt, basket.LastModified);

        foreach (var item in basket.Items)
        {
            var variant = await _mediator.Send(new GetVariantByIdQuery(item.VariantId));

            var image = variant.Options
                .Where(x => x.Image != null)
                .Select(x => x.Image.Url)
                .FirstOrDefault();

            var cartItem = basket.Items.First(x => x.VariantId == variant.Id);
            basketDto.Items.Add(new BasketItemDto(cartItem.VariantId, variant.ProductName ,variant.Title, variant.Price, image, cartItem.Quantity));
        }

        return basketDto;
    }
}
