using DapperOnlineStoreAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DapperOnlineStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromQuery] Guid userId, [FromQuery] string? couponCode)
        {
            var result = await _orderService.OrderCheckoutAsync(userId, couponCode);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] Guid userId)
        {
            var result = await _orderService.GetOrdersAsync(userId);
            return Ok(result);
        }
        [HttpGet("{orderId}/items")]
        public async Task<IActionResult> GetOrderItems(Guid orderId)
        {
            var result = await _orderService.GetOrderItemsAsync(orderId);
            return Ok(result);
        }
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId, Guid userId)
        {
            await _orderService.DeleteOrderAsync(orderId, userId);
            return NoContent();
        }
    }
}
