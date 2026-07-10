using Demo.Domain.Models;
using Demo.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DapperOnlineStoreAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        private Guid GetUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(idClaim);
        }

        [HttpPost("items")]
        public async Task<IActionResult> Add([FromBody] AddCartItemModel add)
        {
            var userId = GetUserId();
            await _cartService.AddToCart(userId, add.ProductId, add.Quantity);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = GetUserId();
            var result = await _cartService.GetCart(userId);
            return Ok(result);
        }
        [HttpPut("items/{productId}")]
        public async Task<IActionResult> Update(Guid productId, [FromBody]UpdateCartModel update)
        {
            var userId = GetUserId();
            await _cartService.UpdateCartItem(userId, productId, update.Quantity);
            return NoContent();
        }
        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> Remove(Guid productId)
        {
            var userId = GetUserId();
            await _cartService.RemoveCartItem(userId, productId);
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveAllCartItems()
        {
            var userId = GetUserId();
            await _cartService.RemoveAllCartItems(userId);
            return NoContent();
        }

    }
}
