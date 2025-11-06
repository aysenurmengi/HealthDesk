using FluentValidation;

namespace HealthDesk.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("DoctorId must be greater than 0.");

            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("PatientId must be greater than 0.");

            RuleFor(x => x.StartsAt)
                .GreaterThan(DateTime.UtcNow).WithMessage("StartsAt must be a future date and time.")
                .Must(BeAtLeastThreeHoursLater).WithMessage("Appointments must be scheduled at least 3 hours in advance.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        }

        private bool BeAtLeastThreeHoursLater(DateTime starsAt)
        {
            return starsAt >= DateTime.UtcNow.AddHours(3);
        }
    }
}