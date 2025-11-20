using Shared.Models.Auth;
using Shared.Models.User;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<TokenResult> Authenticate(SignInRequest request, CancellationToken cancellationToken);
    Task<Guid> Register(SignUpRequest request, CancellationToken cancellationToken);
    Task Logout();
    Task<UserReadModel> GetProfile(CancellationToken cancellationToken);
    Task ConfirmEmail(CancellationToken cancellationToken);
    Task TwoFactor(CancellationToken cancellationToken);
    Task VerifyTwoFactor(CancellationToken cancellationToken);
    Task<TokenResult> RefreshToken(string token, CancellationToken cancellationToken);
    //Task SendResetPassword(SendResetPasswordRequest request, CancellationToken cancellationToken); // send request
    //Task ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken); // verify email address
}
