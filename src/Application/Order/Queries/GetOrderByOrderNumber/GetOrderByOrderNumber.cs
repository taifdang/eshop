using Application.Order.Dtos;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Order.Queries.GetOrderByOrderNumber;

public record GetOrderByOrderNumberQuery(long OrderNumber) : IRequest<CheckoutOrderDto>;

public class GetOrderByOrderNumberQueryHandler : IRequestHandler<GetOrderByOrderNumberQuery, CheckoutOrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderByOrderNumberQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<CheckoutOrderDto> Handle(GetOrderByOrderNumberQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByOrderNumber(request.OrderNumber);

        Guard.Against.NotFound(request.OrderNumber, order);

        return _mapper.Map<CheckoutOrderDto>(order);
    }
}
