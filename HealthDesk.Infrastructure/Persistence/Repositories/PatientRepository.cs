using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthDesk.Infrastructure.Persistence.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly ApplicationDbContext _context;
        public PatientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Patient?> GetByUserIdAsync(int userId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<Patient?> GetPatientWithAppointmentsAsync(int patientId)
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.Id == patientId);
        }


        public async Task<Patient?> GetPatientWithPrescriptionsAsync(int patientId)
        {
            return await _context.Patients
                .Include(p => p.Prescriptions)
                .FirstOrDefaultAsync(p => p.Id == patientId);
        }
    }
}