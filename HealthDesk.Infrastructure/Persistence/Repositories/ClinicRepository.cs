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
    }
}