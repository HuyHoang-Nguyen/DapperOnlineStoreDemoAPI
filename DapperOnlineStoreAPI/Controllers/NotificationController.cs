using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Services;
using DapperOnlineStoreAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DapperOnlineStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationModel n)
        {
            await _notificationService.CreateAsync(n.UserId, n.Message, n.ExpireDate);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetNotifs([FromQuery] Guid userId)
        {
            var notifs = await _notificationService.GetByUserIdAsync(userId);
            return Ok(notifs);
        }
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkRead(Guid id)
        {
            await _notificationService.MarkReadAsync(id);
            return Ok();
        }
        [HttpPut("read-all")]    
        public async Task<IActionResult> MarkAllRead(Guid userId)
        {
            await _notificationService.MarkAllReadAsync(userId);
            return Ok();
        }
    }
}
