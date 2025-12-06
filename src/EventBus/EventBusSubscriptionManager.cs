using System.Text.Json;

namespace EventBus;

public class EventBusSubscriptionManager
{
    public Dictionary<string, Type> EventTypes { get; set; } = new();
    public JsonSerializerOptions JsonOptions { get; set; } = new();
}
