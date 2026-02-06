using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Queries
{
    public sealed record GetDoctorAvailabilityQuery(int DoctorId, DateTime Date)
        : IRequest<IEnumerable<AvailableSlotDto>>;
}
