using Demo.Domain.Models;
using Demo.Domain.Publisher;
using Demo.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperOnlineStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ProductPublisher _productPublisher;
        public ProductController(IProductService productService, ProductPublisher productPublisher)
        {
            _productService = productService;
            _productPublisher = productPublisher;
        }
        [Authorize(Roles = "Admin,Merchant")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductModel p)
        {
            var id = await _productService.CreateAsync(p);
            return Ok(id);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _productService.GetByIdAsync(id);
            _productPublisher.PublishView(new TestRabbit()
            {
                Id = id
            });
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Merchant")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateProductModel p)
        {
            await _productService.UpdateAsync(id, p);
            return NoContent();
        }
        [Authorize(Roles = "Admin,Merchant")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search(string? keyword, Guid? categoryId, decimal? minPrice, decimal? maxPrice, int? minStock, int? maxStock, int page, int pageSize, string sortBy, string sortDir)
        {
            var result = await _productService.SearchAsync(keyword, categoryId, minPrice, maxPrice, minStock, maxStock, page, pageSize, sortBy, sortDir);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Merchant")]
        [HttpPut("event")]
        public async Task<IActionResult> BulkUpdateEvent([FromBody] BulkEventModel p)
        {
            var result = await _productService.BulkUpdateEventAsync(p);
            return Ok(result);
        }
    }
}
