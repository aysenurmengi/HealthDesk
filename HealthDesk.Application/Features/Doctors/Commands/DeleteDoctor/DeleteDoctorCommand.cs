using MediatR;

namespace HealthDesk.Application.Features.Doctors.Commands.DeleteDoctor
{
    public sealed record DeleteDoctorCommand(int Id) : IRequest;
}
