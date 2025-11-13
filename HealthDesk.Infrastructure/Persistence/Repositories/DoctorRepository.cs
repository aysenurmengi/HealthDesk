using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Domain.Entities;
using HealthDesk.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthDesk.Infrastructure.Persistence.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Doctor>> GetByClinicIdAsync(int clinicId)
        {
            return await _context.Doctors
                .Where(d => d.ClinicId == clinicId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetBySpecialtyAsync(SpecialtyType specialty)
        {
            return await _context.Doctors
                .Where(d => d.Specialty == specialty)
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync(DateTime date)
        {
            return await _context.Doctors
                .Where(d => !_context.Appointments
                .Any(a => a.DoctorId == d.Id && a.StartsAt.Date == date.Date))
                .ToListAsync();

        }
    }
}