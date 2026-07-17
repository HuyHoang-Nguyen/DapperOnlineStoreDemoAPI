using Demo.Domain.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.Domain.Services.Workers
{
    public class OrderConsumerWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public OrderConsumerWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<OrderConsumer>();
            await consumer.Start("order_checkout_queue");
        }
    }
}
