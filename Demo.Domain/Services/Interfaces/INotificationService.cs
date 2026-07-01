using Demo.Domain.Entities;

namespace Demo.Domain.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
        Task CreateAsync(Guid? userId, string message, DateTime? expireDate = null);
        Task MarkReadAsync(Guid id);
        Task MarkAllReadAsync(Guid userId);
    }
}
