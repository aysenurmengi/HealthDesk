using HealthDesk.Domain.Common;

namespace HealthDesk.Domain.Entities
{
    public class Prescription : BaseEntity
    {
        public int AppointmentId { get; private set; }
        public Appointment Appointment { get; private set; } = null!;
        public int DoctorId { get; private set; }
        public Doctor Doctor { get; private set; } = null!;
        public int PatientId { get; private set; }
        public Patient Patient { get; private set; } = null!;
        public string Content { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        //content boş olmasın istiyorum
        public Prescription(int appointmentId, int doctorId, int patientId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Prescription content cannot be empty.");

            AppointmentId = appointmentId;
            DoctorId = doctorId;
            PatientId = patientId;
            Content = content;
            AddDomainEvent(new PrescriptionCreatedEvent(this));
        }
        
        
    }
}