using HealthDesk.Domain.Entities;
using HealthDesk.Domain.Enums;

namespace HealthDesk.Application.Common.Interfaces.Repositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetByClinicIdAsync(int clinicId);
        Task<IEnumerable<Doctor>> GetBySpecialtyAsync(SpecialtyType specialty);
        Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync(DateTime date);
    }

}