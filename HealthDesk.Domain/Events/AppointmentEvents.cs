using HealthDesk.Domain.Common;
using HealthDesk.Domain.Entities;

namespace HealthDesk.Domain.Events;
// sealed -> bu sınıftan başka bir sınıf türetilmez, sabit olay
public sealed class AppointmentCreatedEvent(Appointment appointment) : DomainEvent
{
    public Appointment Appointment { get; } = appointment;
}

public sealed class AppointmentApprovedEvent(Appointment appointment) : DomainEvent
{
    public Appointment Appointment { get; } = appointment;
}

public sealed class AppointmentRejectedEvent(Appointment appointment) : DomainEvent
{
    public Appointment Appointment { get; } = appointment;
}

public sealed class AppointmentCompletedEvent(Appointment appointment) : DomainEvent
{
    public Appointment Appointment { get; } = appointment;
}

public sealed class AppointmentRescheduledEvent(Appointment appointment) : DomainEvent
{
    public Appointment Appointment { get; } = appointment;
}
