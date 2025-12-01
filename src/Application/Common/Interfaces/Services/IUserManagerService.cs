using Shared.Models.User;

namespace Application.Common.Interfaces.Services;

public interface IUserManagerService
{
    public Task<List<UserReadModel>> GetList(CancellationToken cancellationToken);
    public Task AssignRole(AssignRoleRequest request, CancellationToken cancellationToken);
    public Task Update(UserUpdateRequest request, CancellationToken cancellationToken);
    public Task Delete(string userId);
}
