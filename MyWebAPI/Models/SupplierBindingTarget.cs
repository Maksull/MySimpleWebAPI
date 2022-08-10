using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models
{
    public class SupplierBindingTarget
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;

        public IEnumerable<Product>? Products { get; set; } = null;

        public Supplier ToSupplier()
        {
            return new Supplier()
            {
                Name = Name,
                City = City,
                Products = Products
            };
        }
    }
}
