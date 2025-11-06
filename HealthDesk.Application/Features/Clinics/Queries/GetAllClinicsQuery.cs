using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Clinics.Queries
{
    public sealed record GetAllClinicsQuery() : IRequest<IEnumerable<ClinicDto>>;
}