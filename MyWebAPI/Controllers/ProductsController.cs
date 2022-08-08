using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(long id)
        {
            Product? p = await _context.Products.FindAsync(id);
            if(p == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(p);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveProduct(ProductBindingTarget productBindingTarget)
        {
            if (ModelState.IsValid)
            {
                Product p = productBindingTarget.ToProduct();
                await _context.Products.AddAsync(p);
                await _context.SaveChangesAsync();
                return Ok(p);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task UpdateProduct(Product product)
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
