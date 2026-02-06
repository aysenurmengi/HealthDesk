using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.Common.Interfaces.Repositories
{
    public interface IDoctorAvailabilityRepository : IRepository<DoctorAvailability>
    {
        Task<IEnumerable<DoctorAvailability>> GetByDoctorIdAsync(int doctorId);
        Task<IEnumerable<DoctorAvailability>> GetByDoctorAndDayAsync(int doctorId, DayOfWeek dayOfWeek);
    }
}
