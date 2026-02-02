using Application.Abstractions;
using Application.Catalog.Products.Queries.GetVariantById;
using Application.Common.Exceptions;
using Application.Customer.Queries.GetCustomerByUserId;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Basket.Commands.UpdateItem;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Guid>
{
    private readonly IBasketRepository _repository;
    private readonly IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProdvider;

    public UpdateItemCommandHandler(
        IBasketRepository repository,
        IMediator mediator,
        ICurrentUserProvider currentUserProdvider)
    {
        _repository = repository;
        _mediator = mediator;
        _currentUserProdvider = currentUserProdvider;
    }

    public async Task<Guid> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserProdvider.GetCurrentUserId();

        if (string.IsNullOrEmpty(userId)) 
            throw new EntityNotFoundException("User not found");
        if (request.Quantity < 0) 
            throw new ArgumentException("Quantity cannot be negative.");

#if (!GrpcOrHttp)
        // Directly call the mediatr handler instead of gRPC / http
        // Here: we call the mediator to get customer and variant details
        var customer = await _mediator.Send(new GetCustomerByUserIdQuery(Guid.Parse(userId)))
                ?? throw new EntityNotFoundException("Customer not found");

        var variant = await _mediator.Send(new GetVariantByIdQuery(request.Id))
                ?? throw new EntityNotFoundException("Product variant not found");
#endif
        if (request.Quantity > variant.Quantity)
        {
            throw new EntityNotFoundException("Not enough product variant quanity");
        }

        // Get or create basket
        var basket = await _repository.GetByCustomerIdWithItemsAsync(customer.Id, cancellationToken);

        if (basket == null)
        {
            basket = new Domain.Entities.Basket()
            {
                CustomerId = customer.Id,
                Items = new List<BasketItem>(),
                CreatedAt = DateTime.UtcNow,
            };
            await _repository.AddAsync(basket, cancellationToken);
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

        basket.UpdatedAt = DateTime.UtcNow;
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return basket.Id;
    }
}