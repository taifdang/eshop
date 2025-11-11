using Application.Basket.Dtos;
using Application.Catalog.Variants.Queries.GetVariantById;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using MediatR;

namespace Application.Basket.Queries.GetBasketByCustomerId;

public record GetBasketByCustomerIdQuery(int CustomerId) : IRequest<BasketDto>;

public class GetBasketByCustomerIdQueryHandler : IRequestHandler<GetBasketByCustomerIdQuery, BasketDto>
{
    private readonly IMediator _mediator;
    private readonly IRepository<Domain.Entities.Basket> _basketRepository;
    public GetBasketByCustomerIdQueryHandler(IMediator mediator, IRepository<Domain.Entities.Basket> basketRepository)
    {
        _mediator = mediator;
        _basketRepository = basketRepository;
    }
    public async Task<BasketDto> Handle(GetBasketByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        // Get basket
        var specification = new BasketCustomerWithItemSpec(request.CustomerId);
        var basket = await _basketRepository.FirstOrDefaultAsync(specification);

        // If basket doesn't exist, return an empty basket
        if (basket == null)
        {
            return new BasketDto
            {
                CustomerId = request.CustomerId,
                Items = new List<BasketItemDto>(),
                CreatedAt = DateTime.UtcNow,
                LastModified = null
            };
        }

        BasketDto vm = new BasketDto { 
            CustomerId = basket.CustomerId,
            Items = new List<BasketItemDto>(),
            CreatedAt = (DateTime)basket.CreatedAt,
            LastModified = basket.LastModified
        };

        foreach (var item in basket.Items)
        {
            var productVariant = await _mediator.Send(new GetVariantByIdQuery { Id = item.ProductVariantId });
            var cartItem = basket.Items.FirstOrDefault(x => x.ProductVariantId == productVariant.Id);

            vm.Items.Add(new BasketItemDto
            {
                ProductVariantId = cartItem.ProductVariantId,
                ProductName = productVariant.Title,
                RegularPrice = productVariant.RegularPrice,
                ImageUrl = productVariant.Image.Url ?? "",
                Quantity = cartItem.Quantity,
            });
        }

        return vm;
    }
}