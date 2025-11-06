using HealthDesk.Application.DTOs;
using HealthDesk.Domain.Enums;
using MediatR;

namespace HealthDesk.Application.Features.Clinics.Commands
{
    public sealed record CreateClinicCommand
    (
        string Name,
        string Address,
        string PhoneNumber,
        SpecialtyType Specialty
    ) : IRequest<ClinicDto>;
}