using Api.Models.Requests;
using Application.Common.Models;
using Application.Identity.Commands.Login;
using Application.Identity.Commands.Logout;
using Application.Identity.Commands.RefreshToken;
using Application.Identity.Commands.RegisterUser;
using Application.Identity.Queries.GetProfile;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Api.Endpoints;

public static class IdentityApi
{
    public static IEndpointRouteBuilder MapIdentityApi (this IEndpointRouteBuilder builder)
    {
        builder.MapGroup("api/v1/identity")
            .MapIdentityApi()
            .WithTags("Identity Api");

        return builder;
    }

    public static RouteGroupBuilder MapIdentityApi(this RouteGroupBuilder group)
    {
        group.MapPost("/register",
            async (IMediator mediator, [FromBody] RegisterUserRequestDto request, CancellationToken cancellationToken) =>
            {
                var command = new RegisterUserCommand(request.UserName, request.Email, request.Password);
                await mediator.Send(command, cancellationToken);
                Results.Created();
            })
            .WithName("RegisterNewUser")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/login",
            async(IMediator mediator, [FromBody] LoginRequestDto request, CancellationToken cancellationToken) =>
            {
                return await mediator.Send(new LoginCommand(request.UserName, request.Password), cancellationToken);
            })
            .WithName("Login")
            .Produces<TokenResult>()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/logout",
             async (IMediator mediator, CancellationToken cancellationToken) =>
             {
                 var command = new LogoutCommand();

                 await mediator.Send(command, cancellationToken);
                 
                 return Results.NoContent();
             })
            .WithName("Logout")
            .Produces<NoContent>()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/refresh-token", 
            async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var command = new RefreshTokenCommand();

                var result = await mediator.Send(command, cancellationToken);

                return result;
            })
            .WithName("Refresh")
            .Produces<TokenResult>()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/profile",
            async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                return await mediator.Send(new GetProfileQuery(), cancellationToken);
            })
            .WithName("Profile")
            .Produces<GetProfileResult>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

        //group.MapPost("/change-password", null);

        //group.MapPost("/forget-password", null);

        //group.MapPost("/confirm-email", null);

        return group;
    }
}
