using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Commands.RejectAppointment
{
    public sealed record RejectAppointmentCommand(int AppointmentId) : IRequest<AppointmentDto>;
}
