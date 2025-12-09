namespace Application.Basket.Queries.GetBasket;

public record BasketItemDto(
    Guid ProductVariantId, 
    string ProductName,
    string VariantName,
    decimal RegularPrice,
    string? ImageUrl,
    int Quantity);
