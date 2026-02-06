using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Commands.ApproveAppointment
{
    public sealed record ApproveAppointmentCommand(int AppointmentId) : IRequest<AppointmentDto>;
}
