namespace Api.Models.Requests;

public record CreateVariantRequestDto(Guid ProductId, decimal RegularPrice, int Quantity);
