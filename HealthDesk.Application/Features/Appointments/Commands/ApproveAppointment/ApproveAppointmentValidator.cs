using FluentValidation;

namespace HealthDesk.Application.Features.Appointments.Commands.ApproveAppointment
{
    public sealed class ApproveAppointmentValidator : AbstractValidator<ApproveAppointmentCommand>
    {
        public ApproveAppointmentValidator()
        {
            RuleFor(x => x.AppointmentId).GreaterThan(0);
        }
    }
}
