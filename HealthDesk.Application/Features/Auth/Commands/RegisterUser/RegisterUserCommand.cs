using HealthDesk.Application.Features.Auth.Dtos;
using MediatR;

namespace HealthDesk.Application.Features.Auth.Commands;
public record RegisterUserCommand(
    string Email,
    string Password,
    string Name
) : IRequest<RegisteredUserDto>;