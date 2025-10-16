using MediatR;

namespace HealthDesk.Domain.Common
{
    public abstract class DomainEvent : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}