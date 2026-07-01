using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Demo.Domain.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;
        protected BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }
        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
