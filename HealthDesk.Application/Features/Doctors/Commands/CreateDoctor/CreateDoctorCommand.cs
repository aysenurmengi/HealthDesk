using HealthDesk.Application.DTOs;
using HealthDesk.Domain.Enums;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Commands.CreateDoctor
{
    public record CreateDoctorCommand(
        string FullName,
        int ClinicId,
        int UserId,
        SpecialtyType Specialty
    ) : IRequest<DoctorDto>;
}