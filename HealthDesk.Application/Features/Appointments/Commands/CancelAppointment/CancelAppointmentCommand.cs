using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Commands.CancelAppointment
{
    public sealed record CancelAppointmentCommand
    (
        int AppointmentId
    ) : IRequest<AppointmentDto>;
}