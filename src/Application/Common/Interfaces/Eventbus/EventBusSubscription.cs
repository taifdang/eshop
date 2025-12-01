namespace Application.Common.Interfaces.Eventbus;

public class EventBusSubscription
{
    public Dictionary<string, Type> EventTypes { get; } = [];
}
