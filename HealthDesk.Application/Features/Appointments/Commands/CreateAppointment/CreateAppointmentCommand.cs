using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Appointments.Commands.CreateAppointment
{
    public sealed record CreateAppointmentCommand
    (
        int DoctorId,
        int PatientId,
        DateTime StartsAt,
        string? Notes
    ) : IRequest<AppointmentDto>;

}