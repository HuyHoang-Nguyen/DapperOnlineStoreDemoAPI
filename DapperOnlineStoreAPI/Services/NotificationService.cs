using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Services.Interfaces;

namespace DapperOnlineStoreAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
        {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }
        public async Task CreateAsync(Guid? userId, string message, DateTime? expireDate)
        {
            await _notificationRepository.CreateAsync(userId, message, expireDate);
        }
        public async Task MarkReadAsync(Guid id)
        {
            await _notificationRepository.MarkReadAsync(id);
        }
        public async Task MarkAllReadAsync(Guid userId)
        {
            await _notificationRepository.MarkAllReadAsync(userId);
        }
    }
}
