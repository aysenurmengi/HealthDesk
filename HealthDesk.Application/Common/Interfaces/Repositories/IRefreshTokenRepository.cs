using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.Common.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task UpdateAsync(RefreshToken token);
    }
}
