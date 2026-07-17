using Demo.Domain.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.Domain.Services.Workers
{
    public class OTPConsumerWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public OTPConsumerWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<OTPConsumer>();
            await consumer.Start("otp_send_queue");
        }
    }
}
