using Demo.Domain.IRepositories;
using Demo.Domain.Publisher;
using Demo.Domain.Services;

namespace Demo.Domain.Consumer
{
    public class ProductConsumer : QueueConsumer<TestRabbit>
    {
        private readonly IProductRepository _productRepository;
        public ProductConsumer(
        IQueueProvider queueProvider, IProductRepository productRepository)
        : base(queueProvider)
        {
            _productRepository = productRepository;
        }
        protected override async Task Handle(TestRabbit testRabbit)
        {
            var product = await _productRepository.GetByIdAsync(testRabbit.Id);
            if (product != null)
            {
                await _productRepository.IncreaseViewCountAsync(testRabbit.Id);
            }
            return;
        }
    }
}
