using HealthDesk.Domain.Common;
using HealthDesk.Domain.Enums;
using static HealthDesk.Domain.Common.ValueObjects;

namespace HealthDesk.Domain.Entities
{
    
    public class User : BaseEntity
    {
        public string FullName { get; private set; } = string.Empty;
        public Email Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } =  string.Empty;
        public UserRole Role { get; private set; }

         private User() {}

        public User(string fullName, Email email, string passwordHash, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name cannot be empty.", nameof(fullName));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));

            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}
