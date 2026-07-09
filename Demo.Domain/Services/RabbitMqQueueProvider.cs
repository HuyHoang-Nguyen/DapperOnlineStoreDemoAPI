using Demo.Domain.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class RabbitMqQueueProvider : IQueueProvider
{
    private readonly ConnectionFactory _factory;

    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqQueueProvider(ConnectionFactory factory)
    {
        _factory = factory;
    }
    private async Task<IChannel> GetChannel()
    {
        if (_connection == null)
        {
            _connection =
                await _factory.CreateConnectionAsync();
        }

        if (_channel == null)
        {
            _channel =
                await _connection.CreateChannelAsync();
        }
        return _channel;
    }
    public async Task Publish<T>(
        string queueName,
        T data)
    {
        var channel = await GetChannel();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);
        var json =
            JsonSerializer.Serialize(data);

        var body =
            Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            body: body);
    }
    public async Task Consume<T>(
        string queueName,
        Func<T, Task> handler)
    {
        var channel = await GetChannel();


        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer =
            new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                var json =
                    Encoding.UTF8.GetString(
                        args.Body.ToArray());

                var message =
                    JsonSerializer.Deserialize<T>(json);

                if (message != null)
                {
                    await handler(message);
                }
                await channel.BasicAckAsync(
                    args.DeliveryTag,
                    false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                await channel.BasicNackAsync(
                    args.DeliveryTag,
                    false,
                    true);
            }
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer);

        Console.WriteLine(
            $"Listening queue: {queueName}");
    }
}