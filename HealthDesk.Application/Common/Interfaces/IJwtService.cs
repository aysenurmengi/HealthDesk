namespace HealthDesk.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string role);
        int? ValidateToken(string token);
    }
}