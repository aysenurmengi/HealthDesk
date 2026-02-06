using HealthDesk.Application.Features.Auth.Dtos;
using HealthDesk.Domain.Enums;
using MediatR;

namespace HealthDesk.Application.Features.Auth.Commands.RegisterDoctor
{
    public sealed record RegisterDoctorCommand(
        string Email,
        string Password,
        string Name,
        int ClinicId,
        SpecialtyType Specialty
    ) : IRequest<RegisteredUserDto>;
}
