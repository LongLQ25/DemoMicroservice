namespace EventBus.Interfaces
{
    public interface IRabbitMQService
    {
        Task PublishAsync<T>(string queueName, T message);
        Task ConsumeAsync<T>(string queueName, Func<T, Task> onMessage);
    }
}
