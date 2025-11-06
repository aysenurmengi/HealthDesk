using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Prescriptions.Queries
{
    public sealed record GetPrescriptionByPatientQuery() : IRequest<IEnumerable<PrescriptionDto>>;
}