using EventBus.Models;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Models;
using NotificationService.Application.Services;
using NotificationService.Application.UseCase.Interfaces;

namespace NotificationService.Application.UseCase.EventHandlers
{
    public class UserRegisteredEventHandler
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UserRegisteredEventHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task HandleAsync(UserRegisteredEvent @event)
        {
            try
            {
                Console.WriteLine($"Received event for {@event.Email}");
                using var scope = _scopeFactory.CreateScope();

                var notificationUseCase = scope.ServiceProvider.GetRequiredService<INotificationUseCase>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                await emailService.SendEmailAsync(@event.Email, "Welcome!", $"Hello {@event.FullName}, welcome!");

                await notificationUseCase.CreateNotificationAsync(new CreateNotificationRequest
                {
                    UserId = @event.UserId,
                    Title = "Welcome",
                    Message = $"Hi {@event.FullName}, your account has been created.",
                    Channel = "Email"
                });

                Console.WriteLine("Handler completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Handler error: {ex}");
            }
        }
    }
}
