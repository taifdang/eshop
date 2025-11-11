using Application.Catalog.Variants.Queries.GetVariantById;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Application.Customer.Queries.GetCustomerByUserId;
using Domain.Entities;
using MediatR;
using Shared.Web;

namespace Application.Basket.Commands.UpdateItem;

public record UpdateItemCommand : IRequest<int>
{
    public Guid UserId { get; set; }
    public int Id { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, int>
{
    private readonly IRepository<Domain.Entities.Basket> _basketRepository;
    private ICurrentUserProdvider _currentUserProvider;
    private readonly IMediator _mediator;
    public UpdateItemCommandHandler(
        ICurrentUserProdvider currentUserProvider, 
        IMediator mediator, 
        IRepository<Domain.Entities.Basket> basketRepository)
    {
        _currentUserProvider = currentUserProvider;
        _mediator = mediator;
        _basketRepository = basketRepository;
    }
    public async Task<int> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        //var userId = _currentUserProvider.GetCurrentUserId()
        //    ?? throw new ArgumentException("User not found");

        if (request.Quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.");

        // Directly call the GetCustomerByUserIdQuery handler instead of gRPC
        var customer = await _mediator.Send(new GetCustomerByUserIdQuery { UserId = request.UserId })
                ?? throw new EntityNotFoundException("Customer not found");

        // Validate ProductVariant exists
        var variant = await _mediator.Send(new GetVariantByIdQuery { Id =  request.Id })
                ?? throw new EntityNotFoundException("Product variant not found");

        // Get or create basket
        var basket = await _basketRepository.FirstOrDefaultAsync(new BasketCustomerWithItemSpec(customer.Id));
        if (basket == null)
        {
            basket = new Domain.Entities.Basket()
            {
                CustomerId = customer.Id,
                Items = new List<BasketItem>()
            };
            //await _basketRepository.AddAsync(basket);

        }
        // Update basket items
        var existingItem = basket.Items.FirstOrDefault(x => x.ProductVariantId == request.Id);

        if (existingItem == null)
        {
            if (request.Quantity > 0)
            {
                basket.Items.Add(new BasketItem()
                {
                    ProductVariantId = request.Id,
                    Quantity = request.Quantity,
                    Price = request.UnitPrice
                });
            }       
        }
        else
        {
            if (request.Quantity > 0)
            {
                existingItem.Quantity = request.Quantity;
            }
            else
            {
                basket.Items.Remove(existingItem);
            }
        }

        basket.LastModified = DateTime.UtcNow;

        await _basketRepository.SaveChangesAsync(cancellationToken);

        return basket.Id;
    }
}