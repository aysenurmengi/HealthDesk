using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.Common.Interfaces.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient?> GetByUserIdAsync(int userId);
        Task<Patient?> GetPatientWithAppointmentsAsync(int patientId);
        Task<Patient?> GetPatientWithPrescriptionsAsync(int patientId);
    }
}