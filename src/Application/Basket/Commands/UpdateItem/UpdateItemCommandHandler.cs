using Application.Catalog.Products.Queries.GetVariantById;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Customer.Queries.GetCustomerByUserId;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Web;

namespace Application.Basket.Commands.UpdateItem;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly ICurrentUserProdvider _currentUserProdvider;
    public UpdateItemCommandHandler(
        IApplicationDbContext dbContext,
        IMediator mediator,
        ICurrentUserProdvider currentUserProdvider)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _currentUserProdvider = currentUserProdvider;
    }

    public async Task<Guid> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserProdvider.GetCurrentUserId();

        if (userId == null)
            throw new ArgumentException("User is required");
        if (request.Quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.");

        // Directly call the GetCustomerByUserIdQuery handler instead of gRPC
        var customer = await _mediator.Send(new GetCustomerByUserIdQuery(Guid.Parse(userId)))
            ?? throw new EntityNotFoundException("Customer not found");

        // Validate Variant exists
        var variant = await _mediator.Send(new GetVariantByIdQuery(request.Id))
                ?? throw new EntityNotFoundException("Product variant not found");

        // Variant quantity not enough
        if (request.Quantity > variant.Quantity)
        {
            throw new EntityNotFoundException("Not enough product variant quanity");
        }

        // Get or create basket
        var basket = await _dbContext.Baskets
           .Include(m => m.Items)
           .FirstOrDefaultAsync(b => b.CustomerId == customer.Id, cancellationToken);

        if (basket == null)
        {
            basket = new Domain.Entities.Basket()
            {
                CustomerId = customer.Id,
                Items = new List<BasketItem>(),
                CreatedAt = DateTime.UtcNow,
            };
            await _dbContext.Baskets.AddAsync(basket);
        }
        // Update basket items
        var existingItem = basket.Items.FirstOrDefault(x => x.VariantId == request.Id);

        if (existingItem == null)
        {
            if (request.Quantity > 0)
            {
                basket.Items.Add(new BasketItem()
                {
                    VariantId = request.Id,
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

        _dbContext.Baskets.Update(basket);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return basket.Id;
    }
}