using DapperOnlineStoreAPI.Entities;

namespace DapperOnlineStoreAPI.IRepositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
        Task CreateAsync(Guid? userId, string message, DateTime? expireDate = null);
        Task MarkReadAsync(Guid id);
        Task MarkAllReadAsync(Guid userId);
    }
}
