using FluentValidation;

namespace HealthDesk.Application.Features.Doctors.Commands.AddDoctorAvailability
{
    public sealed class AddDoctorAvailabilityValidator : AbstractValidator<AddDoctorAvailabilityCommand>
    {
        public AddDoctorAvailabilityValidator()
        {
            RuleFor(x => x.DoctorId).GreaterThan(0);
            RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
        }
    }
}
