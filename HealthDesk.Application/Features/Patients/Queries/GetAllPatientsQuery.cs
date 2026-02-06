using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Patients.Queries
{
    public sealed record GetAllPatientsQuery() : IRequest<IEnumerable<PatientDto>>;
}
