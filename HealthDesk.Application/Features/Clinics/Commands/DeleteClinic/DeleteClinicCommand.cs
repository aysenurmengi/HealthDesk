using MediatR;

namespace HealthDesk.Application.Features.Clinics.Commands.DeleteClinic
{
    public sealed record DeleteClinicCommand(int Id) : IRequest;
}
