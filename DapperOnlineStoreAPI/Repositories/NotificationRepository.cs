using Dapper;
using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.IRepositories;
using System.Data;

namespace DapperOnlineStoreAPI.Repositories
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        public NotificationRepository(IConfiguration configuration) : base(configuration) 
        { 
        }
        public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Notification>("sp_GetNotifs", new { UserId = userId }, commandType: CommandType.StoredProcedure);
        }
        public async Task CreateAsync(Guid? userId, string message, DateTime? expireDate = null)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("sp_CreateNotif", new { UserId = userId, Message = message, ExpireDate = expireDate }, commandType: CommandType.StoredProcedure);
        }
        public async Task MarkReadAsync(Guid id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("sp_ReadNotif", new { Id = id }, commandType: CommandType.StoredProcedure);
        }
        public async Task MarkAllReadAsync(Guid userId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("sp_ReadAllNotif", new { UserId = userId }, commandType: CommandType.StoredProcedure);
        }
    }
}
