using Demo.Domain.IRepositories;
using Demo.Domain.Publisher;
using Demo.Domain.Services;
using Demo.Domain.Services.Interfaces;

namespace Demo.Domain.Consumer
{
    public class OTPConsumer : QueueConsumer<OTPRabbit>
    {
        private readonly IEmailService _emailService;
        public OTPConsumer(IQueueProvider queueProvider, IEmailService emailService) : base(queueProvider) 
        {
            _emailService = emailService;
        }
        protected override async Task Handle(OTPRabbit otpRabbit)
        {
            if (otpRabbit.Email != null && otpRabbit.Code != null)
            {
                await _emailService.SendOTPAsync(otpRabbit.Email, otpRabbit.Code);
            }
        }
    }
}
