using Demo.Domain.Services.Interfaces;

namespace Demo.Domain.Publisher
{
    public class OTPPublisher
    {
        private readonly IQueueProvider _queue;
        public OTPPublisher(IQueueProvider queue)
        {
            _queue = queue;
        }
        public void PublishView(OTPRabbit model)
        {
            _queue.Publish("otp_send_queue", new OTPRabbit()
            {
                 Email = model.Email,
                 Code = model.Code
            });
        }
    }
    public class OTPRabbit
    {
        public string? Email { get; set; }
        public string? Code { get; set; }
    }
}
