using HealthDesk.Domain.Common;
using HealthDesk.Domain.Enums;
using HealthDesk.Domain.Events;

namespace HealthDesk.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public int DoctorId { get; private set; }
        public Doctor Doctor { get; private set; } = null!;
        public int PatientId { get; private set; }
        public Patient Patient { get; private set; } = null!;

        public int ClinicId { get; private set; }
        public Clinic Clinic { get; private set; } = null!;
        public DateTime StartsAt { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string? Notes { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;


        //randevu oluşturma kuralları
        public Appointment(int doctorId, int patientId, DateTime startsAt, string? notes = null)
        {
            if (startsAt <= DateTime.UtcNow)
                throw new InvalidOperationException("Appointment start time must be in the future.");

            if (startsAt < DateTime.UtcNow.AddHours(3))
                throw new InvalidOperationException("Appointments must be scheduled at least 3 hour in advance.");

            DoctorId = doctorId;
            PatientId = patientId;
            StartsAt = startsAt;
            Notes = notes;

            AddDomainEvent(new AppointmentCreatedEvent(this)); //randevu oluşturulduğunda domain event üretilir
        }

        //davranışlar ve event üretimi
        public void Approve()
        {
            if (Status != AppointmentStatus.Requested)
                throw new InvalidOperationException("Only requested appointments can be approved.");

            Status = AppointmentStatus.Approved;
            AddDomainEvent(new AppointmentApprovedEvent(this));
        }

        public void Reject()
        {
            if (Status != AppointmentStatus.Requested)
                throw new InvalidOperationException("Only requested appointments can be rejected.");

            Status = AppointmentStatus.Rejected;
            AddDomainEvent(new AppointmentRejectedEvent(this));
        }

        public void Complete()
        {
            if (Status != AppointmentStatus.Approved)
                throw new InvalidOperationException("Only approved appointments can be completed.");

            Status = AppointmentStatus.Completed;
            AddDomainEvent(new AppointmentCompletedEvent(this));
        }
        
        public void Reschedule(DateTime newDate)
        {
            if (newDate <= DateTime.UtcNow)
                throw new InvalidOperationException("Appointment start time must be in the future.");

            if (newDate < DateTime.UtcNow.AddHours(3))
                throw new InvalidOperationException("Appointments must be scheduled at least 3 hour in advance.");

            StartsAt = newDate;
            AddDomainEvent(new AppointmentRescheduledEvent(this));
        }
    }
}