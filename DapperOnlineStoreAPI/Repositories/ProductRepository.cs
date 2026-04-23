using Dapper;
using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Models.QueryModel;
using System.Data;

namespace DapperOnlineStoreAPI.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(IConfiguration configuration) : base(configuration) 
        { 
        }
        public async Task<bool> CategoryCheckAsync(Guid categoryId)
        {
            using var connection = CreateConnection();
            var sql = "select top 1 1 from Categories where Id = @Id and IsDeleted = 0 ";
            var result = await connection.QueryFirstOrDefaultAsync<int?>(sql, new {Id = categoryId });
            return result.HasValue;
        }
        public async Task<Guid> CreateAsync(ProductModel p)
        {
            using var connection = CreateConnection();
            var sql = "insert into Products(CategoryId, Name, Price, Stock) " +
                "       output inserted.Id " +
                "       values(@CategoryId, @Name, @Price, @Stock) ";
            var product = new
            {
                p.CategoryId,
                p.Name,
                p.Price,
                p.Stock
            };
            return await connection.QuerySingleAsync<Guid>(sql, product);
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = "select * from Products where IsDeleted = 0 ";
            var product = await connection.QueryAsync<Product>(sql);

            return product.Select(p => new Product
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            });
        }
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = "select * from Products where Id = @Id and IsDeleted = 0 ";
            var product = await connection.QueryFirstOrDefaultAsync(sql, new { Id = id});
            if (product == null)
            {
                return null;
            }
            return new Product
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }
        public async Task<int> UpdateAsync(Guid id, UpdateProductModel p)
        {
            using var connection = CreateConnection();
            var sql = "update Products " +
                "      set CategoryId = coalesce(@CategoryId, CategoryId), " +
                "      Name = coalesce(@Name, Name), " +
                "      Price = coalesce(@Price, Price), " +
                "      Stock = coalesce(@Stock, Stock) " +
                "      where Id = @Id and IsDeleted = 0 ";
            return await connection.ExecuteAsync(sql, new
            {
                Id = id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
            });
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = "update Products " +
                "      set IsDeleted = 1 where Id = @Id and IsDeleted = 0 ";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
        public async Task<PagingResult<Product>> SearchAsync(string? keyword, Guid? categoryId, decimal? minPrice, decimal? maxPrice, int? minStock, int? maxStock, int page, int pageSize, string sortBy, string sortDir)
        {
            using var connection = CreateConnection();
            var param = new
            {
                keyword = keyword?.Trim(),
                categoryId = categoryId,
                minPrice = minPrice,
                maxPrice = maxPrice,
                minStock = minStock,
                maxStock = maxStock,
            };
            var result = await connection.QueryAsync<GetProductQueryModel>("sp_SearchProduct", param, commandType: CommandType.StoredProcedure);
            //var pagedResult = result.Select(p => new Product
            //{
            //    Id = p.Id,
            //    CategoryId = p.CategoryId,
            //    CategoryName = p.CategoryName,
            //    Name = p.Name,
            //    Price = p.Price,
            //    Stock = p.Stock
            //});
            IEnumerable<Product> query = result.Select(p => new Product
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            });
            var ascending = sortDir.ToLower() != "desc";
            if (sortBy == "name")
            {
                if (ascending)
                {
                    query = query.OrderBy(p => p.Name);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Name);
                }
            }
            else if (sortBy == "categoryid")
            {
                if (ascending)
                {
                    query = query.OrderBy(p => p.CategoryId);
                }
                else
                {
                    query = query.OrderByDescending(p => p.CategoryId);
                }
            }
            else if (sortBy == "price")
            {
                if (ascending)
                {
                    query = query.OrderBy(p => p.Price);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Price);
                }
            }
            else if (sortBy == "stock")
            {
                if (ascending)
                {
                    query = query.OrderBy(p => p.Stock);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Stock);
                }
            }
            else
            {
                if (ascending)
                {
                    query = query.OrderBy(p => p.Id);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Id);
                }
            }
            var list = query.ToList();
            var totalRecords = list.Count();
            var data = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalPage = (int)Math.Ceiling((double)totalRecords / pageSize);
            return new PagingResult<Product>
            {
                Data = data,
                TotalPages = totalPage,
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<int?> GetStockAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = "select Stock from Products where Id = @Id and IsDeleted = 0 ";
            return await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Id = id });
        }
    }
}
