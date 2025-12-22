using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Identity.Queries.GetProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, GetProfileResult>
{
    private readonly IIdentityService _identityService;

    public GetProfileQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<GetProfileResult> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        return await _identityService.GetProfile();
    }
}
