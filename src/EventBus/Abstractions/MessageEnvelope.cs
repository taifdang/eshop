namespace EventBus.Abstractions;

public class MessageEnvelope
{
    public MessageEnvelope()
    {
    }

    public MessageEnvelope(Type type, string message) : this(type.FullName!, message)
    {
    }

    public MessageEnvelope(string messageTypeName, string message)
    {
        MessageTypeName = messageTypeName ?? throw new ArgumentNullException(nameof(messageTypeName));
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    public string MessageTypeName { get; set; } = default!;
    public string Message { get; set; } = default!;
}
