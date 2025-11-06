using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Commands.UpdateAppointment   
{
    public sealed record UpdateAppointmentCommand
    (
        int AppointmentId,
        DateTime NewStartsAt
    ) : IRequest<AppointmentDto>;
}