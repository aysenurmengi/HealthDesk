using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Prescriptions.Commands.CreatePrescription
{
    public sealed record CreatePrescriptionCommand(
        int AppointmentId,
        string Content
    ) : IRequest<PrescriptionDto>;
}
