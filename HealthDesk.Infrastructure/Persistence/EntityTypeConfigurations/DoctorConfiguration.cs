using HealthDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthDesk.Infrastructure.Persistence.EntityTypeConfiguration
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.FullName)
                .IsRequired()
                .HasMaxLength(100);

            //enum değerini veritabanında sayısal (0,1,2) değil string ("Cardiology") olarak tutar.
            builder.Property(d => d.Specialty)
                .HasConversion<string>() 
                .IsRequired();
      
            builder.HasOne(d => d.Clinic)
                .WithMany(c => c.Doctors)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Kullanıcı ilişkisi (örneğin kimlik doğrulama sistemine bağlıysa)
            builder.Property(d => d.UserId).IsRequired();
        }
    }
}
