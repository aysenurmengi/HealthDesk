using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.Common.Interfaces.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Appointment>> GetCompletedAppointmentsAsync(int doctorId);
        Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime startsAt, CancellationToken cancellationToken = default);
        void UpdateRange(IEnumerable<Appointment> appointments);
    }
}