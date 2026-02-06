using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Queries
{
    public sealed record GetAllDoctorsQuery() : IRequest<IEnumerable<DoctorDto>>;
}
