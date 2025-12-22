using Application.Common.Models;
using MediatR;

namespace Application.Identity.Queries.Login;

public record LoginQuery(string UserName, string Password) : IRequest<TokenResult>;
