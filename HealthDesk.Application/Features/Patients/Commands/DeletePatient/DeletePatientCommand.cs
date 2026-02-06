using MediatR;

namespace HealthDesk.Application.Features.Patients.Commands.DeletePatient
{
    public sealed record DeletePatientCommand(int Id) : IRequest;
}
