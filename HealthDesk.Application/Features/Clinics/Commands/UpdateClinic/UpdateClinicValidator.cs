using FluentValidation;

namespace HealthDesk.Application.Features.Clinics.Commands.UpdateClinic
{
    public class UpdateClinicValidator : AbstractValidator<UpdateClinicCommand>
    {
        public UpdateClinicValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.City).NotEmpty().MaximumLength(100);
            RuleFor(x => x.District).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(250);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Specialty).IsInEnum();
        }
    }
}
