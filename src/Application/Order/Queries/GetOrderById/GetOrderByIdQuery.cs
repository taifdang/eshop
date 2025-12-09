using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Order.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderItemDto>;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderItemDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<OrderItemDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId);

        return _mapper.Map<OrderItemDto>(order);
    }
}
