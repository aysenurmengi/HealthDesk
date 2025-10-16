using HealthDesk.Domain.Common;
using HealthDesk.Domain.Enums;
using static HealthDesk.Domain.Common.ValueObjects;

namespace HealthDesk.Domain.Entities
{
    //hasta ve doktor için ortak kullanıcı sınıfı
    public class User : BaseEntity
    {
        public string FullName { get; private set; } = string.Empty;
        public Email Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } =  string.Empty;
        public UserRole Role { get; private set; }
    }
}