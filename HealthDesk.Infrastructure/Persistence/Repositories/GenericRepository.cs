using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace HealthDesk.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public async Task DeleteAsync(T entity) => await Task.Run(() => _context.Set<T>().Remove(entity));

        public async Task<IReadOnlyList<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

        public async Task UpdateAsync(T entity) => await Task.Run(() => _context.Set<T>().Update(entity));
    }
}