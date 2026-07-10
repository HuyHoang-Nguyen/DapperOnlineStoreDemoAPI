using Demo.Domain.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.Domain.Services.Workers
{
    public class ProductConsumerWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public ProductConsumerWorker(
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(
            CancellationToken cancellationToken)
        {
            using var scope =
                _scopeFactory.CreateScope();

            var consumer =
                scope.ServiceProvider
                .GetRequiredService<ProductConsumer>();

            await consumer.Start(
                "product_view_queue");
        }
    }
}
