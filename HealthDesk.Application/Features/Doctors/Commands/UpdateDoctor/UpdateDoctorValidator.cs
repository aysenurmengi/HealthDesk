using FluentValidation;

namespace HealthDesk.Application.Features.Doctors.Commands.UpdateDoctor
{
    public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorCommand>
    {
        public UpdateDoctorValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ClinicId).GreaterThan(0);
            RuleFor(x => x.Specialty).IsInEnum();
        }
    }
}
