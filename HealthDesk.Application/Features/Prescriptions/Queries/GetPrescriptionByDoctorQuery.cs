using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Prescriptions.Queries
{
    public sealed record GetPrescriptionByDoctorQuery() : IRequest<IEnumerable<PrescriptionDto>>;
}