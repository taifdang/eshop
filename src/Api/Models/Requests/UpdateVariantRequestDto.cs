namespace Api.Models.Requests;

public record UpdateVariantRequestDto(Guid Id, decimal RegularPrice, int Quantity);
