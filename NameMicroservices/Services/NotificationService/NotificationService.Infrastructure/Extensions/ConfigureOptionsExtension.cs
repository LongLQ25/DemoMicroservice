using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Options;

namespace NotificationService.Infrastructure.Extensions
{
    public static class ConfigureOptionsExtension
    {
        public static IServiceCollection AddConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)));
            _ = services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));

            return services;
        }
    }
}
