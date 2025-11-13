using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthDesk.Infrastructure.Persistence.Repositories
{
    public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
    {
        private readonly ApplicationDbContext _context;
        public PrescriptionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Prescription?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Prescriptions.FirstOrDefaultAsync(p => p.AppointmentId == appointmentId);
        }

        public async Task<IEnumerable<Prescription>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.Prescriptions
                .Where(p => p.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Prescriptions
                .Where(p => p.PatientId == patientId)
                .ToListAsync();
        }
    }
}