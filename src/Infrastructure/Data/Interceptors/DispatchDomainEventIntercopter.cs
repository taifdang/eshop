using Domain.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Data.Interceptors;

public class DispatchDomainEventIntercopter : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DispatchDomainEventIntercopter(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        DispatchDomainEvent(eventData.Context).GetAwaiter().GetResult();

        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvent(eventData.Context);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public async Task DispatchDomainEvent(DbContext? context)
    {
        if (context == null) return;

        var domainEnitites = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEnitites
            .SelectMany(x => x.DomainEvents)
            .ToList();

        domainEnitites.ForEach(entity => entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
    }
}
