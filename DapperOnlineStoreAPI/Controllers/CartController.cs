using DapperOnlineStoreAPI.IRepositories;
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
            await _cartService.AddToCart(add.ProductId, add.Quantity);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _cartService.GetCart();
            return Ok(result);
        }
        [HttpPut("items/{productId}")]
        public async Task<IActionResult> Update(Guid productId, [FromBody]UpdateCartModel update)
        {
            await _cartService.UpdateCartItem(productId, update.QuantityChange);
            return Ok();
        }
        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> Remove(Guid productId)
        {
            await _cartService.RemoveCartItem(productId);
            return Ok();
        }
    }
}
