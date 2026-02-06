using FluentValidation;

namespace HealthDesk.Application.Features.Appointments.Commands.RejectAppointment
{
    public sealed class RejectAppointmentValidator : AbstractValidator<RejectAppointmentCommand>
    {
        public RejectAppointmentValidator()
        {
            RuleFor(x => x.AppointmentId).GreaterThan(0);
        }
    }
}
