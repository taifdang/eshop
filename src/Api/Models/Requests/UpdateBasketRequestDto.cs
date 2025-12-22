namespace Api.Models.Requests;

public record UpdateBasketRequestDto(Guid VariantId, int Quantity);