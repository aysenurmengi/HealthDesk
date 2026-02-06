using HealthDesk.Application.DTOs;
using HealthDesk.Domain.Enums;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Commands.UpdateDoctor
{
    public sealed record UpdateDoctorCommand(
        int Id,
        string FullName,
        int ClinicId,
        SpecialtyType Specialty
    ) : IRequest<DoctorDto>;
}
