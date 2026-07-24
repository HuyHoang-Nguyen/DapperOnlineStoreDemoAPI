using Microsoft.AspNetCore.Http;

namespace Demo.Domain.Models
{
    public class UpdateProductModel
    {

        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public decimal? Discount { get; set; }
        public DateTime? DiscountStart { get; set; }
        public DateTime? DiscountEnd { get; set; }
        public decimal? EventDiscount { get; set; }
        public DateTime? EventStart { get; set; }
        public DateTime? EventEnd { get; set; }
        public List<IFormFile>? Images { get; set; }
        public string? ImageUrl { get; set; }
    }
}