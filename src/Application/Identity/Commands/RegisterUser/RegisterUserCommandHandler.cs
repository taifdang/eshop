using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Identity.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IIdentityService _identityService;

    public RegisterUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<RegisterUserResult> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _identityService.Register(new RegisterUserRequest(command.UserName, command.Email, command.Password));

        return result;
    }
}
