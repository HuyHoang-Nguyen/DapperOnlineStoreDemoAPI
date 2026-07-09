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
        private readonly IUserService _userService;
        private readonly IOTPService _otpService;
        private readonly IJwtService _jwtService;
        public AuthController(IAuthService authService, IUserService userService, IOTPService otpService, IJwtService jwtService)
        {
            _authService = authService;
            _userService = userService;
            _otpService = otpService;
            _jwtService = jwtService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            await _authService.LoginAsync(login.Email, login.Password);
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
