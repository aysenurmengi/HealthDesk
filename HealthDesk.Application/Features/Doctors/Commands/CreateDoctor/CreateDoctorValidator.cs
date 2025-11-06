using FluentValidation;

namespace HealthDesk.Application.Features.Doctors.Commands.CreateDoctor
{
    public class CreateDoctorValidator : AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorValidator()
        {
            RuleFor(x => x.Specialty)
                .NotEmpty().WithMessage("Specialty is required."); 
        }
    }
}