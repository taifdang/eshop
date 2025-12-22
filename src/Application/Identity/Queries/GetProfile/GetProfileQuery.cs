using Application.Common.Models;
using MediatR;

namespace Application.Identity.Queries.GetProfile;

public record GetProfileQuery() : IRequest<GetProfileResult>;