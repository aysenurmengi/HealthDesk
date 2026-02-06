using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthDesk.Infrastructure.Persistence.Repositories
{
    public class ClinicRepository : GenericRepository<Clinic>, IClinicRepository
    {
        private readonly ApplicationDbContext _context;
        public ClinicRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Clinic>> GetClinicsWithDoctorsAsync()
        {
            return await _context.Clinics
                .Include(c => c.Doctors)
                .ToListAsync();
        }

        public async Task<IEnumerable<Clinic>> GetByCityAsync(string city)
        {
            return await _context.Clinics
                .Where(c => c.City.ToLower() == city.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetCitiesAsync()
        {
            return await _context.Clinics
                .Select(c => c.City)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }
    }
}
