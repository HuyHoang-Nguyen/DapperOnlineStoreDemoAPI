using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DapperOnlineStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("items")]
        public async Task<IActionResult> Add([FromBody] AddCartItemModel add)
        {
            await _cartService.AddToCart(add.UserId, add.ProductId, add.Quantity);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]Guid userId)
        {
            var result = await _cartService.GetCart(userId);
            return Ok(result);
        }
        [HttpPut("items/{productId}")]
        public async Task<IActionResult> Update([FromQuery]Guid userId, Guid productId, [FromBody]UpdateCartModel update)
        {
            await _cartService.UpdateCartItem(userId, productId, update.Quantity);
            return NoContent();
        }
        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> Remove([FromQuery]Guid userId, Guid productId)
        {
            await _cartService.RemoveCartItem(userId, productId);
            return NoContent();
        }
    }
}
