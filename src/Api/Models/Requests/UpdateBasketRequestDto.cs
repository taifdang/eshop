namespace Api.Models.Requests;

public record UpdateBasketRequestDto(Guid AccountId, Guid VariantId, int Quantity);