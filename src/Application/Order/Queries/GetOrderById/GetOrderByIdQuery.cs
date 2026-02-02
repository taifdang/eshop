using Domain.Repositories;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Application.Order.Dtos;

namespace Application.Order.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<List<OrderItemDto>>;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, List<OrderItemDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<List<OrderItemDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId);

        Guard.Against.NotFound(request.OrderId, order);

        return _mapper.Map<List<OrderItemDto>>(order.Items);
    }
}
