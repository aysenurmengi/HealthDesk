namespace HealthDesk.Application.Features.Doctors.Commands.CreateDoctor
{
    public sealed record CreateDoctorCommand
    (
        string FirstName,
        string LastName,
        string Specialty,
        string Email,
        string PhoneNumber
    );
}