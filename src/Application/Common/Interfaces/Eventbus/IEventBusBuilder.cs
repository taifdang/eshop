using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Interfaces.Eventbus;

public interface IEventBusBuilder
{
    public IServiceCollection Services { get; }
}
