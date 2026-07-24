using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Demo.Domain.Models
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
        public decimal? Discount {  get; set; }
        public DateTime? DiscountStart { get; set; }
        public DateTime? DiscountEnd { get; set; }
        public decimal? EventDiscount { get; set; }
        public DateTime? EventStart { get; set; }
        public DateTime? EventEnd { get; set; }
        public List<IFormFile>? Images { get; set; }
        public string? ImageUrl { get; set; }
    }
}