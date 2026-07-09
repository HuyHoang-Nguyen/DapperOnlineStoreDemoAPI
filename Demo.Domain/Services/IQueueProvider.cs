namespace Demo.Domain.Services
{
    public interface IQueueProvider
    {
        Task Consume<T>(
        string queueName,
        Func<T, Task> handler);

        Task Publish<T>(
         string queueName,
         T data);
    }
}
