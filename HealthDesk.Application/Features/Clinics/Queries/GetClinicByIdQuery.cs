using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Clinics.Queries
{
    public sealed record GetClinicByIdQuery(int Id) : IRequest<ClinicDto>;
    
}