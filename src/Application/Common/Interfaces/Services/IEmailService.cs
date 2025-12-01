namespace Application.Common.Interfaces.Services;

public interface IEmailService
{
    Task SendAsync(string To, string Subject, string Message);
}
