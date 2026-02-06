using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Queries
{
    public sealed record GetMyAppointmentsQuery() : IRequest<IEnumerable<AppointmentDto>>;
}
