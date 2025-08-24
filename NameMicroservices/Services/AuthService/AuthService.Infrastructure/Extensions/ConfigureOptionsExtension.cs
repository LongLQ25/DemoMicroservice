using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthService.Infrastructure.Options;

namespace AuthService.Infrastructure.Extensions
{
    public static class ConfigureOptionsExtension
    {
        public static IServiceCollection AddConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            _ = services.Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)));

            return services;
        }
    }
}
