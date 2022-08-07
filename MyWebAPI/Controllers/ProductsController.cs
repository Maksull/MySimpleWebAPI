using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("/api/{controller}")]
    public class ProductsController : Controller
    {
        private DataContext _context;

        public ProductsController([FromServices] DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IAsyncEnumerable<Product> GetProducts()
        {
            return _context.Products.AsAsyncEnumerable();
        }

        [HttpGet("{id}")]
        public async Task<Product> GetProduct(long id)
        {
            return await _context.Products.FindAsync(id);
        }

        [HttpPost]
        public async Task SaveProduct([FromBody] ProductBindingTarget productBindingTarget)
        {
            await _context.Products.AddAsync(productBindingTarget.ToProduct());
            await _context.SaveChangesAsync();
        }

        [HttpPut]
        public async Task UpdateProduct([FromBody] Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task DeleteProduct(long id)
        {
            _context.Products.Remove(new Product() { ProductId = id});
            await _context.SaveChangesAsync();
        }
    }
}
