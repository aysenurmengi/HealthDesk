using HealthDesk.Application.Common.Interfaces.Repositories;

namespace HealthDesk.Application.Common.Interfaces
{
    //tek transaction ile birden fazla işlem yürütmek için
    public interface IUnitOfWork
    {
        IAppointmentRepository Appointments { get; }
        IDoctorRepository Doctors { get; }
        IPatientRepository Patients { get; }
        IClinicRepository Clinics { get; }
        IPrescriptionRepository Prescriptions { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}