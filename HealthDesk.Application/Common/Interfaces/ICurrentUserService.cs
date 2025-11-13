namespace HealthDesk.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }        // JWT içinden gelen UserId
        string? Role { get; }       // Kullanıcının rolü (Admin, Doctor, Patient)
        string? Email { get; }      // Kullanıcının e-postası
    }
}