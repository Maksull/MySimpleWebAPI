using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models
{
    public class ProductBindingTarget
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(1, 1500)]
        public decimal Price { get; set; }
        [Range(1, long.MaxValue)]
        public long CategoryId { get; set; }
        [Range(1, long.MaxValue)]
        public long SupplierId { get; set; }

        public Product ToProduct()
        {
            return new Product()
            {
                Name = Name,
                Price = Price,
                CategoryId = CategoryId,
                SupplierId = SupplierId
            };
        }
    }
}
