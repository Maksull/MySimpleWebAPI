namespace MyWebAPI.Models
{
    public class ProductBindingTarget
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public long CategoryId { get; set; }
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
