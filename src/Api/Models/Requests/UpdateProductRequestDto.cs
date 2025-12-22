namespace Api.Models.Requests;

public record UpdateProductRequestDto(Guid Id, Guid CategoryId, string Title, string Description);
