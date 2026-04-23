using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DapperOnlineStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _loggingService;
        public AuthController(IAuthService loggingService)
        {
            _loggingService = loggingService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var result = await _loggingService.LoginAsync(login.Email, login.Password);
            return Ok(result);
        }
    }
}
