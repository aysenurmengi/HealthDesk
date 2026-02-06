using HealthDesk.Domain.Common;

namespace HealthDesk.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; private set; } = string.Empty;
        public int UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? ReplacedByToken { get; private set; }

        private RefreshToken() {}

        public RefreshToken(
            string token,
            int userId,
            DateTime createdAt,
            DateTime expiresAt)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Refresh token cannot be empty.", nameof(token));

            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), "UserId must be a positive number.");

            if (expiresAt <= createdAt)
                throw new ArgumentException("ExpiresAt must be after CreatedAt.", nameof(expiresAt));

            Token = token;
            UserId = userId;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
        }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsRevoked => RevokedAt != null;

        public bool IsActive => !IsExpired && !IsRevoked;

        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }

        public void ReplaceWith(string newToken)
        {
            ReplacedByToken = newToken;
            Revoke();
        }
    }
}
