using FluentValidation;

namespace HealthDesk.Application.Features.Auth.Commands.RegisterPatient
{
    public sealed class RegisterPatientValidator : AbstractValidator<RegisterPatientCommand>
    {
        public RegisterPatientValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        }
    }
}
