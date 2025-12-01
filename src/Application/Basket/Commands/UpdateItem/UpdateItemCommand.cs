using Application.Basket.Specifications;
using Application.Catalog.Products.Queries.GetVariantById;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Customer.Queries.GetCustomerByUserId;
using Domain.Entities;
using MediatR;
using Shared.Web;

namespace Application.Basket.Commands.UpdateItem;

public record UpdateItemCommand(Guid Id, int Quantity) : IRequest<Guid>;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Guid>
{
    private readonly IRepository<Domain.Entities.Basket> _basketRepo;
    private readonly IMediator _mediator;
    private readonly ICurrentUserProdvider _currentUserProdvider;
    public UpdateItemCommandHandler(
        IRepository<Domain.Entities.Basket> basketRepo,
        IMediator mediator,
        ICurrentUserProdvider currentUserProdvider)
    {
        _basketRepo = basketRepo;
        _mediator = mediator;
        _currentUserProdvider = currentUserProdvider;
    }

    public async Task<Guid> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserProdvider.GetCurrentUserId();

        if(userId == null)
            throw new ArgumentException("User is required");
        if (request.Quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.");

        // Directly call the GetCustomerByUserIdQuery handler instead of gRPC
        var customer = await _mediator.Send(new GetCustomerByUserIdQuery(Guid.Parse(userId)))
            ?? throw new EntityNotFoundException("Customer not found");

        // Validate ProductVariant exists
        var variant = await _mediator.Send(new GetVariantByIdQuery(request.Id))
                ?? throw new EntityNotFoundException("Product variant not found");

        // ProductVariant quantity not enough
        if(request.Quantity > variant.Quantity)
        {
            throw new EntityNotFoundException("Not enough product variant quanity");
        }

        // Get or create basket
        var spec = new BasketSpec()
             .ByCustomerId(customer.Id)
             .WithItems();

        var basket = await _basketRepo.FirstOrDefaultAsync(spec);

        if (basket == null)
        {
            basket = new Domain.Entities.Basket()
            {
                CustomerId = customer.Id,
                Items = new List<BasketItem>(),
                CreatedAt = DateTime.UtcNow,
            };
            await _basketRepo.AddAsync(basket);
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

        //await _basketRepo.SaveChangesAsync(cancellationToken);

        return basket.Id;
    }
}