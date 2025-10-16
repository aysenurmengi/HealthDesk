using HealthDesk.Domain.Common;

namespace HealthDesk.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public string FullName { get; private set; } = string.Empty;
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;

        public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
        public ICollection<Prescription> Prescriptions { get; private set; } = new List<Prescription>();

        public Patient(string fullName, int userId)
        {
            FullName = fullName;
            UserId = userId;
        }
    }
}