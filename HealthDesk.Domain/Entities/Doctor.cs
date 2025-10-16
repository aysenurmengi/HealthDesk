using HealthDesk.Domain.Common;
using HealthDesk.Domain.Enums;

namespace HealthDesk.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public string FullName { get; private set; } = string.Empty;
        public int ClinicId { get; private set; }
        public Clinic Clinic { get; private set; } = null!;
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;
        public SpecialtyType Specialty { get; private set; }

        public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
        public ICollection<Prescription> Prescriptions { get; private set; } = new List<Prescription>();

        public Doctor(string fullName, int clinicId, int userId, SpecialtyType specialty)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name cannot be empty.", nameof(fullName));

            if (!Enum.IsDefined(typeof(SpecialtyType), specialty) || specialty == 0)
                throw new ArgumentException("A doctor must have one valid specialty.", nameof(specialty));

            FullName = fullName;
            ClinicId = clinicId;
            UserId = userId;
            Specialty = specialty;
        }


    }
}