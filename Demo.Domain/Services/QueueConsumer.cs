namespace Demo.Domain.Services
{
    public abstract class QueueConsumer<T>
    {
        private readonly IQueueProvider _queueProvider;

        protected QueueConsumer(IQueueProvider queueProvider)
        {
            _queueProvider = queueProvider;
        }
        public async Task Start(string queueName)
        {
            await _queueProvider.Consume<T>(
               queueName,
               async message =>
               {
                   await Handle(message);
               });
        }
        protected abstract Task Handle(T message);
    }
}
