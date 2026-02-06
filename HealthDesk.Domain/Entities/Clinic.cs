using HealthDesk.Domain.Common;
using HealthDesk.Domain.Enums;

namespace HealthDesk.Domain.Entities
{
    public class Clinic : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string District { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;

        //ilerde alan bazlı seçim eklersem diye;
        public SpecialtyType Specialty { get; private set; }

        public ICollection<Doctor> Doctors { get; private set; } = new List<Doctor>();

        private Clinic() { }

        public Clinic(string name, string city, string district, string address, string phoneNumber, SpecialtyType specialty)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Clinic name cannot be empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("Clinic city cannot be empty.", nameof(city));
            if (string.IsNullOrWhiteSpace(district))
                throw new ArgumentException("Clinic district cannot be empty.", nameof(district));
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Clinic address cannot be empty.", nameof(address));
            
            Name = name;
            City = city;
            District = district;
            Address = address;
            PhoneNumber = phoneNumber;
            Specialty = specialty;
        }

        //değişiklik yapmak istediğimde tekrar entity oluşturmak yerine bu methodu kullanırım
        public void UpdateDetails(string name, string city, string district, string address, string phoneNumber, SpecialtyType specialty)
        {
            Name = name;
            City = city;
            District = district;
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
