using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.Common.Interfaces.Repositories
{
    public interface IPrescriptionRepository : IRepository<Prescription>
    {
        Task<Prescription?> GetByAppointmentIdAsync(int appointmentId);
        Task<IEnumerable<Prescription>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Prescription>> GetByDoctorIdAsync(int doctorId);
    }
    
}