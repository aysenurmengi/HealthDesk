using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.Common.Interfaces.Repositories;
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
}
