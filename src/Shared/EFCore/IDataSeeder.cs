namespace Shared.EFCore;

public interface IDataSeeder
{
    Task SendAllAsync();
}
