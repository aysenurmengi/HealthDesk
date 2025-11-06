using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Patients.Queries
{
    public sealed record GetPatientByIdQuery(int Id) : IRequest<PatientDto>;
}