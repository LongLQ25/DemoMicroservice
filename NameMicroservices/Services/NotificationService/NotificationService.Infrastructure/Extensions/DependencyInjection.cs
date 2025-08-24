using EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationService.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddConfigureOptions(configuration);
            _ = services.AddCustomDbContext(configuration);
            _ = services.AddEventBus(configuration);
            _ = services.AddServices();

            return services;
        }

    }
}
