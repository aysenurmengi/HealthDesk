using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.Common.Interfaces.Repositories;
using HealthDesk.Infrastructure.Persistence;
using HealthDesk.Infrastructure.Repositories;
using HealthDesk.Infrastructure.Security;
using HealthDesk.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthDesk.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserRepository, UserRepository>();


            services.AddHttpContextAccessor();

            return services;
        }
    }
}
