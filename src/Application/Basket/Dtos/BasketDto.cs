namespace Application.Basket.Dtos;

public record BasketDto(
    Guid Id, 
    Guid CustomerId,
    List<BasketItemDto> Items,
    DateTime CreatedAt, 
    DateTime? LastModified);
