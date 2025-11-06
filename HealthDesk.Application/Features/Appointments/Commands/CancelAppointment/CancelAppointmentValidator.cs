using FluentValidation;

namespace HealthDesk.Application.Features.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentValidator : AbstractValidator<CancelAppointmentCommand>
    {
        public CancelAppointmentValidator()
        {
            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("AppointmentId must be greater than 0.");
        }
    }
}