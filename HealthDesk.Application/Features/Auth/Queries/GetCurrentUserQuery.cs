using HealthDesk.Application.Features.Auth.Dtos;
using MediatR;

namespace HealthDesk.Application.Features.Auth.Queries;

public record GetCurrentUserQuery : IRequest<CurrentUserDto>;
