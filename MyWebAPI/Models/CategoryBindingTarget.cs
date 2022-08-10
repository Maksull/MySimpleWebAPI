using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models
{
    public class CategoryBindingTarget
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<Product>? Products { get; set; } = null;

        public Category ToCategory()
        {
            return new Category()
            {
                Name = Name,
                Products = Products
            };
        }
    }
}
