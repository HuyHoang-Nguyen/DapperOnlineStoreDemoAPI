using Demo.Domain.Enum;
using Demo.Domain.IRepositories;
using Demo.Domain.Publisher;
using Demo.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DapperOnlineStoreAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepository;
        private readonly OrderPublisher _orderPublisher;
        private readonly ICartRepository _cartRepository;
        public OrderController(IOrderService orderService, IOrderRepository orderRepository, OrderPublisher orderPublisher, ICartRepository cartRepository)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
            _orderPublisher = orderPublisher;
            _cartRepository = cartRepository;
        }
        private Guid GetUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(idClaim);
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromQuery] string? couponCode)
        {
            var userId = GetUserId();
            var cartItems = await _cartRepository.GetCart(userId);
            _orderPublisher.PublishView(new OrderRabbit { UserId = userId, CouponCode = couponCode, CartItems = cartItems.ToList()});
            return Accepted();
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = GetUserId();
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
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var userId = GetUserId();
            await _orderService.DeleteOrderAsync(orderId, userId);
            return NoContent();
        }
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromQuery] EnumOrderStatus status)
        {
            await _orderRepository.UpdateOrderStatusAsync(orderId, status);
            return NoContent();
        }
    }
}
