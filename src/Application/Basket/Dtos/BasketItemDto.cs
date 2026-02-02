namespace Application.Basket.Dtos;

public record BasketItemDto(
    Guid ProductVariantId, 
    string ProductName,
    string VariantName,
    decimal RegularPrice,
    string? ImageUrl,
    int Quantity);
