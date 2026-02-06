using FluentValidation;

namespace HealthDesk.Application.Features.Prescriptions.Commands.CreatePrescription
{
    public class CreatePrescriptionValidator : AbstractValidator<CreatePrescriptionCommand>
    {
        public CreatePrescriptionValidator()
        {
            RuleFor(x => x.AppointmentId).GreaterThan(0);
            RuleFor(x => x.Content).NotEmpty().MaximumLength(1000);
        }
    }
}
