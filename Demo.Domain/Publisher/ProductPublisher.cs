using Demo.Domain.Services;

namespace Demo.Domain.Publisher
{
    public class ProductPublisher
    {
        private readonly IQueueProvider _queue;

        public ProductPublisher(IQueueProvider queue)
        {
            _queue = queue;
        }
        public void PublishView(TestRabbit model)
        {
            _queue.Publish("product_view_queue", new TestRabbit()
            {
                Id = model.Id,
                Name = model.Name
            });
        }
    }
    public class TestRabbit
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
