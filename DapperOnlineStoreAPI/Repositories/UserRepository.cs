using Dapper;
using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Models.QueryModel;

namespace DapperOnlineStoreAPI.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<bool> EmailExistedAsync(string email)
        {
            using var connection = CreateConnection();
            var sql = @"select top 1 1 from Users where Email = @Email and IsDeleted = 0";
            var result = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Email = email });
            return result.HasValue;
        }
        public async Task<Guid> CreateAsync(UserModel u)
        {
            using var connection = CreateConnection();
            var sql = "insert into Users(Email, Password) " +
                "      output inserted.Id " +
                "      values(@Email, @Password) ";
            var user = new
            {
                u.Email,
                u.Password
            };
            return await connection.QuerySingleAsync<Guid>(sql, user);
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = "Select * from Users where IsDeleted = 0";
            var users = await connection.QueryAsync<GetUserQueryModel>(sql);

            return users.Select(us => new User
            {
                Id = us.Id,
                Email = us.Email,
                Password = us.Password
            });
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = "Select * from Users where Id = @Id and IsDeleted = 0";
            var u = await connection.QueryFirstOrDefaultAsync<GetUserQueryModel?>(sql, new { Id = id });
            if (u == null)
            {
                return null;
            }
            return new User
            {
                Id = u.Id,
                Email = u.Email,
                Password = u.Password
            };
        }
        public async Task<int> UpdateAsync(Guid id, UpdateUserModel u)
        {
            using var connection = CreateConnection();
            var sql = "update Users " +
                "      set Password = @Password " +
                "      where Id = @Id and IsDeleted = 0 ";
            return await connection.ExecuteAsync(sql, new
            {
                Id = id,
                Password = u.Password
            });
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = "update Users " +
                "      set IsDeleted = 1 where Id = @Id and IsDeleted = 0 ";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = CreateConnection();
            var sql = "select Id, Email, Password from Users where Email = @Email ";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }
    }
}
