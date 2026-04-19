namespace DapperOnlineStoreAPI.Models
{
    public class PagingResult<T>
    {
        public IEnumerable<T>? Data { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}