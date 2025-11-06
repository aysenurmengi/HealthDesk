using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.Common.Interfaces.Repositories
{
    public interface IClinicRepository : IRepository<Clinic>
    {
        Task<IEnumerable<Clinic>> GetClinicsWithDoctorsAsync();
    }
}