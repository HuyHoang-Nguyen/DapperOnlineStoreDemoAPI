using DapperOnlineStoreAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DapperOnlineStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        [HttpGet("validate")]
        public async Task<IActionResult> Validate([FromQuery] string code, [FromQuery] decimal cartTotal, [FromQuery] Guid userId)
        {
            var result = await _couponService.ValidateAsync(code, cartTotal, userId);
            return Ok(result);
        }
    }
}
