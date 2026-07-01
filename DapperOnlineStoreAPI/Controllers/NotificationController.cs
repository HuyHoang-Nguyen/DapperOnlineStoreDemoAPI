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
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        private Guid GetUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(idClaim);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationModel n)
        {
            var userId = GetUserId();
            await _notificationService.CreateAsync(userId, n.Message, n.ExpireDate);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetNotifs()
        {
            var userId = GetUserId();
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
        public async Task<IActionResult> MarkAllRead()
        {
            var userId = GetUserId();
            await _notificationService.MarkAllReadAsync(userId);
            return Ok();
        }
    }
}
