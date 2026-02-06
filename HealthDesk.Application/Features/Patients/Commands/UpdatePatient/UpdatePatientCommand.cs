using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Patients.Commands.UpdatePatient
{
    public sealed record UpdatePatientCommand(
        int Id,
        string FullName
    ) : IRequest<PatientDto>;
}
