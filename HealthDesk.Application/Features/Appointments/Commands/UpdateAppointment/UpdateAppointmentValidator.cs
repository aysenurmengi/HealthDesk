using FluentValidation;

namespace HealthDesk.Application.Features.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentValidator()
        {
            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("AppointmentId must be greater than 0.");

            RuleFor(x => x.NewStartsAt)
                .GreaterThan(DateTime.UtcNow).WithMessage("NewStartsAt must be a future date and time.")
                .Must(BeAtLeastThreeHoursLater).WithMessage("Appointments must be scheduled at least 3 hours in advance.");
        }

        private bool BeAtLeastThreeHoursLater(DateTime newStartsAt)
        {
            return newStartsAt >= DateTime.UtcNow.AddHours(3);
        }
    }
}