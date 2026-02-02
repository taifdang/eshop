using Domain.Repositories;
using Application.Customer.Queries.GetCustomerByUserId;
using AutoMapper;
using MediatR;
using Application.Order.Dtos;
using Application.Abstractions;

namespace Application.Order.Queries.GetListOrder;

public record GetListOrderQuery() : IRequest<List<OrderListDto>>;

public class GetListOrderQueryHandler : IRequestHandler<GetListOrderQuery, List<OrderListDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IMediator _mediator;

    public GetListOrderQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper,
        ICurrentUserProvider currentUserProvider,
        IMediator mediator)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
        _mediator = mediator;
    }

    public async Task<List<OrderListDto>> Handle(GetListOrderQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserProvider?.GetCurrentUserId();

        if(string.IsNullOrEmpty(userId))
        {
            throw new Exception("UnAuthorized");
        }

        var customerId = await _mediator.Send(new GetCustomerByUserIdQuery(Guid.Parse(userId)));

        if(customerId is null)
        {
            throw new Exception("Not found customer");
        }

        var orders = await _orderRepository.GetListByCustomerAsync(customerId.Id);

        return _mapper.Map<List<OrderListDto>>(orders);
    }
}
