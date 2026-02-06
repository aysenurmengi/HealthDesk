using HealthDesk.Application.Features.Auth.Dtos;
using MediatR;

namespace HealthDesk.Application.Features.Auth.Commands.RegisterPatient
{
    public sealed record RegisterPatientCommand(
        string Email,
        string Password,
        string Name
    ) : IRequest<RegisteredUserDto>;
}
