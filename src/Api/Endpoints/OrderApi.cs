using Api.Models.Requests;
using Application.Order.Commands.CancelOrder;
using Application.Order.Commands.CreateOrder;
using Application.Order.Queries.GetListOrder;
using Application.Order.Queries.GetOrderById;
using MediatR;

namespace Api.Endpoints;

public static class OrderApi
{
    public static IEndpointRouteBuilder MapOrderApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGroup("api/v1/orders")
            .MapOrderApi()
            .WithTags("Order Api");

        return builder;
    }

    public static RouteGroupBuilder MapOrderApi(this RouteGroupBuilder group)
    {
        group.MapGet("/",
            async(IMediator mediator, CancellationToken cancellationToken) =>
            {
                return await mediator.Send(new GetListOrderQuery(), cancellationToken);
            })
            .RequireAuthorization()
            .WithName("GetOrder")
            .WithSummary("Get order of current user")
            .Produces<List<OrderListDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet($"/{{id}}",
            async (IMediator mediator, Guid orderId, CancellationToken cancellationToken) =>
            {
                var command = new GetOrderByIdQuery(orderId);

                return await mediator.Send(command, cancellationToken);
            })
            .RequireAuthorization()
            .WithName("GetOrderById")
            .Produces<List<OrderItemDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/",
            async (IMediator mediator, [AsParameters] CreateOrderRequestDto request, CancellationToken cancellationToken) =>
            {
                var command = new CreateOrderCommand(request.CustomerId, request.Street, request.City, request.ZipCode);

                var result = await mediator.Send(command, cancellationToken);

                return result;
            })
            .RequireAuthorization()
            .WithName("CreateOrder")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/",
            async (IMediator mediator, Guid orderId, CancellationToken cancellationToken) =>
            {
                var command = new CancelOrderCommand(orderId);

                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("CancelOrder")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }
}
