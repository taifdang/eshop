using Application.Common.Models;
using MediatR;

namespace Application.Identity.Commands.RefreshToken;

public record RefreshTokenCommand() : IRequest<TokenResult>;