using MediatR;

namespace HealthDesk.Application.Features.Clinics.Queries
{
    public sealed record GetClinicCitiesQuery() : IRequest<IEnumerable<string>>;
}
