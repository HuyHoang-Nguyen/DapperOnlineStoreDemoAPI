using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DapperOnlineStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserModel u)
        {
            var id = await _userService.CreateAsync(u);
            return Ok(id);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserModel u)
        {
            await _userService.UpdateAsync(id, u);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("email")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _userService.GetByEmailAsync(email);
            return Ok(result);
        }
    }
}
