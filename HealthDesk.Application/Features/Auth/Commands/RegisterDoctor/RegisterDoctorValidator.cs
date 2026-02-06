using FluentValidation;

namespace HealthDesk.Application.Features.Auth.Commands.RegisterDoctor
{
    public sealed class RegisterDoctorValidator : AbstractValidator<RegisterDoctorCommand>
    {
        public RegisterDoctorValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ClinicId).GreaterThan(0);
            RuleFor(x => x.Specialty).IsInEnum();
        }
    }
}
