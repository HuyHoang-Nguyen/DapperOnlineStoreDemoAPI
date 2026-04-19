using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperOnlineStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddToCartModel add)
        {
            await _cartRepository.AddToCart(add.ProductId, add.Quantity);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _cartRepository.GetCart();
            return Ok(data);
        }
    }
}
