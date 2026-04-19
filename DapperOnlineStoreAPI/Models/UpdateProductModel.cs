using System.ComponentModel.DataAnnotations;

namespace DapperOnlineStoreAPI.Models
{
    public class UpdateProductModel
    {

        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int Stock { get; set; }
    }
}
