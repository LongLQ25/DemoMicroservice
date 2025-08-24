using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Repositories;
using NotificationService.Application.Services;
using NotificationService.Application.UseCase.EventHandlers;
using NotificationService.Infrastructure.Persistence.Repositories;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Infrastructure.Extensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            _ = services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            _ = services.AddScoped<INotificationRepository, NotificationRepository>();

            _ = services.AddScoped<IEmailService, EmailService>();
            _ = services.AddHostedService<RabbitMQBackgroundConsumer>();

            return services;
        }
    }
}
