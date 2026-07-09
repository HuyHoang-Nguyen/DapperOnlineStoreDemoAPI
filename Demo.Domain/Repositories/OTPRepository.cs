using Dapper;
using Demo.Domain.Entities;
using Demo.Domain.IRepositories;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Demo.Domain.Repositories
{
    public class OTPRepository : BaseRepository, IOTPRepository
    {
        public OTPRepository(IConfiguration configuration) : base(configuration) 
        {
        }
        public async Task CreateAsync(string email, string code, DateTime expireAt)
        {
            using var connection = CreateConnection();         
            await connection.ExecuteAsync("sp_CreateOTP", new { Email = email, Code = code, ExpireAt = expireAt }, commandType: CommandType.StoredProcedure);
        }

        public async Task<OTPCode?> GetOTPAsync(string email, string code)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<OTPCode>("sp_GetOTP", new { Email = email, Code = code }, commandType: CommandType.StoredProcedure);
        }

        public async Task UsedOTPAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = "update OTPCodes set IsUsed = 1 where Id = @Id and IsUsed = 0 ";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
