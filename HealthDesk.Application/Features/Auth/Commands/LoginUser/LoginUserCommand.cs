using HealthDesk.Application.Features.Auth.Dtos;
using MediatR;

namespace HealthDesk.Application.Features.Auth.Commands;

public record LoginUserCommand(
    string Email,
    string Password
) : IRequest<LoginUserDto>;
