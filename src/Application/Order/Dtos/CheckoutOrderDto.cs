using AutoMapper;
using Domain.Enums;

namespace Application.Order.Dtos;

public class CheckoutOrderDto
{
    public Guid Id { get; set; }
    public long OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public Guid CustomerId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentProvider PaymentProvider { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Order, CheckoutOrderDto>()
                .ForMember(d => d.TotalAmount, s => s.MapFrom(opt => opt.TotalAmount.Amount))
                .ForMember(d => d.Currency, s => s.MapFrom(opt => opt.TotalAmount.Currency));
        }
    }
}
