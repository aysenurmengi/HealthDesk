using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.Common.Interfaces.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime startsAt);
        Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);
        Task<IEnumerable<Appointment>> GetCompletedAppointmentsAsync(int doctorId);
        Task IsDoctorAvailableAsync(int doctorId, DateTime startsAt, CancellationToken cancellationToken);
        Task UpdateAsync(IEnumerable<Appointment> appointment);
    }
}