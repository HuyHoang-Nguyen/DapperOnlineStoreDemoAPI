using System.ComponentModel.DataAnnotations;

namespace DapperOnlineStoreAPI.Models
{
    public class ProductModel
    {
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
    }
}
