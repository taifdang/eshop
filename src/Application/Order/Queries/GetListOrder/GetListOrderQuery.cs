using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Order.Queries.GetListOrder;

public record GetListOrderQuery : IRequest<List<OrderListDto>>;

public class GetListOrderQueryHandler : IRequestHandler<GetListOrderQuery, List<OrderListDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetListOrderQueryHandler(
        IOrderRepository orderRepository, 
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<List<OrderListDto>> Handle(GetListOrderQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetListAsync();

        return _mapper.Map<List<OrderListDto>>(orders);
    }
}
