using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Identity.Commands.RefreshToken;

public record RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResult>
{
    private readonly IIdentityService _identityService;

    public RefreshTokenCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<TokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.RefreshToken();
    }
}
