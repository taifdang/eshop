using AutoMapper;
using Domain.Enums;

namespace Application.Order.Dtos;

public class OrderListDto
{
    public Guid Id { get; set; }
    public long OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Order, OrderListDto>()
                .ForMember(d => d.TotalAmount, s => s.MapFrom(opt => opt.TotalAmount.Amount));
        }
    }
}
