namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendAsync(string To, string Subject, string Message);
}
