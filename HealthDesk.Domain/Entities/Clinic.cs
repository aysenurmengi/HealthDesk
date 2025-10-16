using HealthDesk.Domain.Common;
using HealthDesk.Domain.Enums;

namespace HealthDesk.Domain.Entities
{
    public class Clinic : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;

        //ilerde alan bazlı seçim eklersem diye;
        public SpecialtyType Specialty { get; private set; }

        public ICollection<Doctor> Doctors { get; private set; } = new List<Doctor>();

        public Clinic(string name, string address, string phoneNumber, SpecialtyType specialty)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Clinic name cannot be empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Clinic address cannot be empty.", nameof(address));
            
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Specialty = specialty;
        }

        //değişiklik yapmak istediğimde tekrar entity oluşturmak yerine bu methodu kullanırım
        public void UpdateDetails(string name, string address, string phoneNumber, SpecialtyType specialty)
        {
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Specialty = specialty;
        }

        public void AddDoctor(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));

            if (Doctors.Any(d => d.Id == doctor.Id))
                throw new InvalidOperationException("Doctor is already added to the clinic.");

            Doctors.Add(doctor);
        }
    }
}
