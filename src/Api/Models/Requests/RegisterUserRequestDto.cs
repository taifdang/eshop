namespace Api.Models.Requests;

public record RegisterUserRequestDto(string UserName, string Email, string Password);