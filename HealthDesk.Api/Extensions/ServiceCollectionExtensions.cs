using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace HealthDesk.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Enum’ları string olarak serialize etmek için
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                    // Null değerleri gizleme
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                    //Property adlarını PascalCase yerine camelCase??
                    // options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            // CORS politikası
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
