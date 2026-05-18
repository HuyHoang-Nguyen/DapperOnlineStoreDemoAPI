using Dapper;
using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.IRepositories;

namespace DapperOnlineStoreAPI.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = "select Id, Name from Categories where IsDeleted = 0 order by Name ";
            var category = await connection.QueryAsync<Category>(sql);
            return category;
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = "select Id, Name from Categories where Id = @Id and IsDeleted = 0 ";
            var category = await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id} );
            return category;
        }
    }
}
