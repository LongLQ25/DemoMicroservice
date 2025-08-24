using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.UseCase.EventHandlers;
using NotificationService.Application.UseCase.Implementaions;
using NotificationService.Application.UseCase.Interfaces;

namespace NotificationService.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            _ = services.AddScoped<INotificationUseCase, NotificationUseCase>();
            _ = services.AddSingleton<UserRegisteredEventHandler>();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddUseCases();

            return services;
        }
    }
}
