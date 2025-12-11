using System.Security.Claims;
using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}