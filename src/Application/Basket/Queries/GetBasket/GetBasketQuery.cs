using Application.Basket.Dtos;
using Application.Catalog.Variants.Queries.GetVariantById;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Application.Customer.Queries.GetCustomerByUserId;
using MediatR;

namespace Application.Basket.Queries.GetCartList;

public record GetBasketQuery(Guid UserId) : IRequest<BasketDto>;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, BasketDto>
{
    private readonly IRepository<Domain.Entities.Basket> _basketRepository;
    private readonly IMediator _mediator;
    public GetBasketQueryHandler(IRepository<Domain.Entities.Basket> baskerRepository, IMediator mediator)
    {
        _basketRepository = baskerRepository;
        _mediator = mediator;
    }
    public async Task<BasketDto> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {      
        // Directly call the GetCustomerByUserIdQuery handler instead of gRPC
        var customer = await _mediator.Send(new GetCustomerByUserIdQuery { UserId = request.UserId })
                ?? throw new EntityNotFoundException("Customer not found");

        // Get basket
        var basket = await _basketRepository.FirstOrDefaultAsync(new BasketCustomerWithItemSpec(customer.Id));

        // If basket doesn't exist, return an empty basket
        if (basket == null)
        {
            return new BasketDto
            {
                CustomerId = customer.Id,
                Items = new List<BasketItemDto>(),
                CreatedAt = DateTime.UtcNow,
                LastModified = null
            };
        }

        BasketDto vm = new BasketDto
        {
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
