using Demo.Domain.IRepositories;
using Demo.Domain.Publisher;
using Demo.Domain.Services;

namespace Demo.Domain.Consumer
{
    public class OTPConsumer : QueueConsumer<OTPRabbit>
    {
        private readonly IOTPRepository _otpRepository;
        public OTPConsumer(IOTPRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }
        protected override async Task Handle(OTPRabbit otpRabbit)
        {
        }
    }
}
