using Infrastructure.Messaging.InMemory;

namespace Infrastructure.Messaging;

public class MessagingOptions
{
    public string Provider { get; set; }
    public InMemoryOptions InMemory { get; set; }
    public bool UseInmemory()
    {
        return Provider == "InMemory";
    }
    // Add other provider checks here
}
