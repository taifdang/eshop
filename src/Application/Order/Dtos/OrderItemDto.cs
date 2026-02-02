using AutoMapper;

namespace Application.Order.Dtos;

public class OrderItemDto
{
    public Guid OrderId { get; set; }
    public Guid VariantId { get; set; }
    public string Name { get; set; } = default!;
    public string Title { get; set; } = default!;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
    public string ImageUrl { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.OrderItem, OrderItemDto>()
                .ForMember(d => d.Name, s => s.MapFrom(opt => opt.ProductName))
                .ForMember(d => d.Title, s => s.MapFrom(opt => opt.VariantTitle));
        }
    }
}
