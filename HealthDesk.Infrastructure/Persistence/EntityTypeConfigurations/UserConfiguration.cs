using HealthDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthDesk.Infrastructure.Persistence.EntityTypeConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(200);

            // 💡 Email Value Object olarak konfigüre ediliyor
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Address)
                    .HasColumnName("Email")      // DB'de kolon adı "Email" olsun
                    .IsRequired()
                    .HasMaxLength(200);
            });

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.Role)
                .HasConversion<string>()        // Enum -> string olarak kaydet
                .IsRequired();
        }
    }
}
