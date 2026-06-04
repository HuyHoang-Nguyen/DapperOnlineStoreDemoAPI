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
        private static decimal? CalcDiscountedPrice(decimal price, decimal? discount)
        {
            if (discount == null || discount <= 0)
            {
                return null;
            }
            return Math.Round(price * (1 - discount.Value / 100), 0);
        }
        private static bool IsDiscountActive(DateTime? start, DateTime? end)
        {
            if (start.HasValue && DateTime.Now < start.Value) return false;
            if (end.HasValue && DateTime.Now > end.Value) return false;
            return true;
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
            var sql =   "insert into Products(CategoryId, Name, Price, Stock, Discount, DiscountStart, DiscountEnd) " +
                "       output inserted.Id " +
                "       values(@CategoryId, @Name, @Price, @Stock, @Discount, @DiscountStart, @DiscountEnd) ";
            var product = new
            {
                p.CategoryId,
                p.Name,
                p.Price,
                p.Stock,
                p.Discount,
                p.DiscountStart,
                p.DiscountEnd
            };
            return await connection.QuerySingleAsync<Guid>(sql, product);
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = " select p.Id, p.CategoryId, c.Name as CategoryName, p.Name, p.Price, p.Stock, p.ImageURL, p.Discount, p.DiscountStart, p.DiscountEnd, p.EventDiscount, p.DiscountStart, p.DiscountEnd " +
                      " from Products p " +
                      " left join Categories c on c.Id = p.CategoryId and c.IsDeleted = 0 " +
                      " where p.IsDeleted = 0 ";
            var product = await connection.QueryAsync<Product>(sql);

            return product.Select(p =>
            {
                var eventActive = IsDiscountActive(p.EventStart, p.EventEnd) && p.EventDiscount.HasValue;
                var discountActive = IsDiscountActive(p.DiscountStart, p.DiscountEnd) && p.Discount.HasValue;

                var finalDiscount = eventActive ? p.EventDiscount : discountActive ? p.Discount : null;

                var endDate = eventActive ? p.EventEnd : discountActive ? p.DiscountEnd : null;

                return new Product
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    CategoryName = p.CategoryName,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    ImageUrls = (p.ImageUrl ?? "").Split('|', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Discount = finalDiscount,
                    DiscountPrice = CalcDiscountedPrice(p.Price, finalDiscount),
                    DiscountStart = p.DiscountStart,
                    DiscountEnd = endDate,
                    EventDiscount = p.EventDiscount,
                    EventStart = p.EventStart,
                    EventEnd = p.EventEnd,
                };
            });
        }
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = " select p.Id, p.CategoryId, c.Name as CategoryName, p.Name, p.Price, p.Stock, p.ImageURL, p.Discount, p.DiscountStart, p.DiscountEnd, p.EventDiscount, p.DiscountStart, p.DiscountEnd, p.EventDiscount, p.EventStart, p.EventEnd " +
                      " from Products p " +
                      " left join Categories c on c.Id = p.CategoryId and c.IsDeleted = 0 " +
                      " where p.Id = @Id and p.IsDeleted = 0 ";
            var product = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
            if (product == null)
            {
                return null;
            }
            var eventActive = IsDiscountActive(product.EventStart, product.EventEnd) && product.EventDiscount.HasValue;
            var discountActive = IsDiscountActive(product.DiscountStart, product.DiscountEnd) && product.Discount.HasValue;

            var finalDiscount = eventActive ? product.EventDiscount : discountActive ? product.Discount : null;

            var endDate = eventActive ? product.EventEnd : discountActive ? product.DiscountEnd : null;

            return new Product
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                CategoryName = product.CategoryName,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                ImageUrls = (product.ImageUrl ?? "").Split('|', StringSplitOptions.RemoveEmptyEntries).ToList(),
                Discount = finalDiscount,
                DiscountPrice = CalcDiscountedPrice(product.Price, finalDiscount),
                DiscountStart = product.DiscountStart,
                DiscountEnd = endDate,
                EventDiscount = product.EventDiscount,
                EventStart = product.EventStart,
                EventEnd = product.EventEnd,
            };
        }
        public async Task<int> UpdateAsync(Guid id, UpdateProductModel p)
        {
            using var connection = CreateConnection();
            var sql = "update Products " +
                "      set CategoryId = coalesce(@CategoryId, CategoryId), " +
                "      Name = coalesce(@Name, Name), " +
                "      Price = coalesce(@Price, Price), " +
                "      Stock = coalesce(@Stock, Stock), " +
                "      Discount = coalesce(@Discount, Discount), " +
                "      DiscountStart = coalesce(@DiscountStart, DiscountStart), " +
                "      DiscountEnd = coalesce(@DiscountEnd, DiscountEnd), " +
                "      EventDiscount = coalesce(@EventDiscount, EventDiscount), " +
                "      EventStart = coalesce(@EventStart, EventStart), " +
                "      EventEnd = coalesce(@EventEnd, EventEnd) " +
                "      where Id = @Id and IsDeleted = 0 ";
            return await connection.ExecuteAsync(sql, new
            {
                Id = id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                Discount = p.Discount,
                DiscountStart = p.DiscountStart,
                DiscountEnd = p.DiscountEnd,
                EventDiscount = p.EventDiscount,
                EventStart = p.EventStart,
                EventEnd = p.EventEnd,
            });
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = "update Products " +
                "      set IsDeleted = 1 where Id = @Id and IsDeleted = 0 " +
                "      @declare @AffectedRows int = @@rowcount ";
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
            IEnumerable<Product> query = result.Select(p =>
            {
                var eventActive = IsDiscountActive(p.EventStart, p.EventEnd) && p.EventDiscount.HasValue;
                var discountActive = IsDiscountActive(p.DiscountStart, p.DiscountEnd) && p.Discount.HasValue;

                var finalDiscount = eventActive ? p.EventDiscount : discountActive ? p.Discount : null;

                var endDate = eventActive ? p.EventEnd : discountActive ? p.DiscountEnd : null;

                return new Product
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    CategoryName = p.CategoryName,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    ImageUrls = (p.ImageUrl ?? "").Split('|', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Discount = finalDiscount,
                    DiscountPrice = CalcDiscountedPrice(p.Price, finalDiscount),
                    DiscountStart = p.DiscountStart,
                    DiscountEnd = endDate,
                    EventDiscount = p.EventDiscount,
                    EventStart = p.EventStart,
                    EventEnd = p.EventEnd,
                };
            });
            var ascending = sortDir.ToLower() != "desc";
            query = query.OrderBy(p => p.Stock <= 0);

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

        public async Task<int> BulkUpdateEventAsync(List<Guid> productIds, decimal eventDiscount, DateTime? eventStart, DateTime? eventEnd)
        {
            if (productIds == null || !productIds.Any()) return 0;
            using var connection = CreateConnection();
            var sql = "update Products " +
                      "set EventDiscount = @EventDiscount, " +
                      "    EventStart = @EventStart, " +
                      "    EventEnd = @EventEnd " +
                      "where Id in @Ids and IsDeleted = 0";
            return await connection.ExecuteAsync(sql, new {EventDiscount = eventDiscount, EventStart = eventStart, EventEnd = eventEnd, Ids = productIds });
        }
    }
}
