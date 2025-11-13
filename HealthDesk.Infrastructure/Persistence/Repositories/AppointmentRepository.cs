using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Domain.Entities;
using HealthDesk.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthDesk.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetCompletedAppointmentsAsync(int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Completed)
                .ToListAsync();
        }

        public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime startsAt, CancellationToken cancellationToken = default)
        {
            return !await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId && a.StartsAt == startsAt, cancellationToken);
        }

        public void UpdateRange(IEnumerable<Appointment> appointments)
        {
            _context.Appointments.UpdateRange(appointments);
        }
    }
}