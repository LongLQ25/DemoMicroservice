using EventBus.Interfaces;
using EventBus.Models;
using Microsoft.Extensions.Hosting;
using NotificationService.Application.UseCase.EventHandlers;

namespace NotificationService.Infrastructure.Services
{
    public class RabbitMQBackgroundConsumer : BackgroundService
    {
        private readonly IRabbitMQService _rabbitMQ;
        private readonly UserRegisteredEventHandler _handler;

        public RabbitMQBackgroundConsumer(IRabbitMQService rabbitMQ, UserRegisteredEventHandler handler)
        {
            _rabbitMQ = rabbitMQ;
            _handler = handler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine("RabbitMQBackgroundConsumer is stopping...");
                    return;
                }

                Console.WriteLine("RabbitMQBackgroundConsumer started...");
                await _rabbitMQ.ConsumeAsync<UserRegisteredEvent>("user.registered", async message =>
                {
                    await _handler.HandleAsync(message);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQBackgroundConsumer error: {ex}");
            }
        }
    }
}
