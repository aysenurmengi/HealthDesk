using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Queries
{
    public sealed record GetDoctorsByClinicQuery(int ClinicId) : IRequest<IEnumerable<DoctorDto>>;
}
