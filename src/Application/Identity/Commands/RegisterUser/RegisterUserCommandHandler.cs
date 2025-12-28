using Application.Common.Interfaces;
using Application.Common.Models;
using Contracts.IntegrationEvents;
using EventBus.Abstractions;
using MediatR;

namespace Application.Identity.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IIdentityService _identityService;
    private readonly IEventPublisher _eventPublisher;

    public RegisterUserCommandHandler(IIdentityService identityService, IEventPublisher eventPublisher)
    {
        _identityService = identityService;
        _eventPublisher = eventPublisher;
    }

    public async Task<RegisterUserResult> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _identityService.Register(new RegisterUserRequest(command.UserName, command.Email, command.Password));

        await _eventPublisher.PublishAsync(new UserCreatedIntegrationEvent
        {
            UserId = result.Id,
            Email = result.Email,
        });

        return result;
    }
}
