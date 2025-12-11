using Microsoft.EntityFrameworkCore;
using HealthDesk.Domain.Entities;
using HealthDesk.Domain.Common;
using System.Security;

namespace HealthDesk.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Clinic> Clinics => Set<Clinic>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Ignore<DomainEvent>();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }

}