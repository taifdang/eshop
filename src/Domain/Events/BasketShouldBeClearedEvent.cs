using Domain.Common;

namespace Domain.Events;

public record BasketShouldBeClearedEvent(Guid CustomerId) : IDomainEvent;