using Api.Models.Requests;
using Application.Basket.Commands.UpdateItem;
using Application.Basket.Queries.GetBasket;
using Application.Basket.Queries.GetCartList;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints;

public static class BasketApi
{
    public static IEndpointRouteBuilder MapBasketApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGroup("/api/v1/basket")
            .MapBasketApi()
            .WithTags("Basket Api");
        return builder;
    }

    public static RouteGroupBuilder MapBasketApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            return await mediator.Send(new GetBasketQuery());
        })
        .RequireAuthorization()
        .WithName("GetBasket")
        .WithSummary("Get basket of current user")
        .Produces<BasketDto>()
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/", async (IMediator mediator, [AsParameters] UpdateBasketRequestDto request, CancellationToken cancellationToken) =>
        {
            await mediator.Send(new UpdateItemCommand(request.VariantId, request.Quantity));

            return Results.NoContent();
        })
       .RequireAuthorization()
       .WithName("UpdateBasket")
       .WithSummary("Update basket of current user")
       .Produces<NoContent>()
       .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }
}
