using HealthDesk.Domain.Common;

namespace HealthDesk.Domain.Entities;

public sealed class PrescriptionCreatedEvent(Prescription prescription) : DomainEvent
{
    public Prescription Prescription { get; } = prescription;
}