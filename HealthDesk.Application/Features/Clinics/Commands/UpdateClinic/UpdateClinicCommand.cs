using HealthDesk.Application.DTOs;
using HealthDesk.Domain.Enums;
using MediatR;

namespace HealthDesk.Application.Features.Clinics.Commands.UpdateClinic
{
    public sealed record UpdateClinicCommand(
        int Id,
        string Name,
        string City,
        string District,
        string Address,
        string PhoneNumber,
        SpecialtyType Specialty
    ) : IRequest<ClinicDto>;
}
