using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Infrastructure.Persistence.Repositories;

namespace HealthDesk.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IClinicRepository? _clinicRepository;
        private IPatientRepository? _patientRepository;
        private IDoctorRepository? _doctorRepository;
        private IAppointmentRepository? _appointmentRepository;
        private IPrescriptionRepository? _prescriptionRepository;
        
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IAppointmentRepository Appointments => _appointmentRepository ??= new AppointmentRepository(_context);

        public IDoctorRepository Doctors => _doctorRepository ??= new DoctorRepository(_context);

        public IPatientRepository Patients => _patientRepository ??= new PatientRepository(_context);

        public IClinicRepository Clinics => _clinicRepository ??= new ClinicRepository(_context);

        public IPrescriptionRepository Prescriptions => _prescriptionRepository ??= new PrescriptionRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}