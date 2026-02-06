using MediatR;

namespace HealthDesk.Application.Features.Auth.Commands
{
    public sealed record LogoutUserCommand(string RefreshToken) : IRequest;
}
