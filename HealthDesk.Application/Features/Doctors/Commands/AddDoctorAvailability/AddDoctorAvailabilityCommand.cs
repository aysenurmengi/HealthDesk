using MediatR;

namespace HealthDesk.Application.Features.Doctors.Commands.AddDoctorAvailability
{
    public sealed record AddDoctorAvailabilityCommand(
        int DoctorId,
        DayOfWeek DayOfWeek,
        TimeSpan StartTime,
        TimeSpan EndTime
    ) : IRequest;
}
