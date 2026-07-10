using Demo.Domain.Models;
using Demo.Domain.Publisher;
using Demo.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DapperOnlineStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly OTPPublisher _otpPublisher;
        public AuthController(IAuthService authService, OTPPublisher otpPublisher)
        {
            _authService = authService;
            _otpPublisher = otpPublisher;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var code = await _authService.LoginAsync(login.Email, login.Password);
            _otpPublisher.PublishView(new OTPRabbit { Email = login.Email, Code = code });
            return Ok(new { email = login.Email, otpRequired = true});
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPModel otp)
        {
            var result = await _authService.VerifyOTPAsync(otp.Email, otp.Code);
            return Ok(result);
        }
    }
}
