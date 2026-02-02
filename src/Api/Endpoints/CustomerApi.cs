using Application.Abstractions;
using Application.Customer.Dtos;
using Application.Customer.Queries.GetCustomerByUserId;
using MediatR;

namespace Api.Endpoints;

public static class CustomerApi
{
    public static IEndpointRouteBuilder MapCustomerApi(this IEndpointRouteBuilder builder) 
    {
        builder.MapGroup("/api/v1/customers")
            .MapCustomerApi()
            .WithTags("Customer Api");

        return builder;
    }

    public static RouteGroupBuilder MapCustomerApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", 
            async (IMediator mediator, ICurrentUserProvider currentUserProvider, CancellationToken cancellationToken) =>
            {
                var userId = currentUserProvider.GetCurrentUserId();

                var result = await mediator.Send(new GetCustomerByUserIdQuery(Guid.Parse(userId)));

                return result;
            })
            .RequireAuthorization()
            .WithName("GetCustomer")
            .WithSummary("Get infomation of current user")
            .Produces<CustomerDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }
}
