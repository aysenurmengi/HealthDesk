using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Queries
{
    public sealed record GetAllAppointmentsQuery() : IRequest<IEnumerable<AppointmentDto>>;
}