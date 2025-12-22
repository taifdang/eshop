namespace Api.Models.Responses;

public record RegisterUserResult(Guid Id, string UserName, string Email);
