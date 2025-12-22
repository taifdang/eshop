using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Identity.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, TokenResult>
{
    private readonly IIdentityService _identityService;

    public LoginQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<TokenResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        return await _identityService.Authenticate(new LoginRequest(request.UserName, request.Password));
    }
}
