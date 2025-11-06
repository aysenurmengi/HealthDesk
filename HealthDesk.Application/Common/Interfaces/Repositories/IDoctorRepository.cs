using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.Common.Interfaces.Repositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetByClinicIdAsync(int clinicId);
        Task<IEnumerable<Doctor>> GetBySpecialtyAsync(string specialty);
        Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync(DateTime date);
    }

}