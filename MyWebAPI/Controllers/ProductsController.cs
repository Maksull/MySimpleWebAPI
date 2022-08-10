using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class ProductsController : Controller
    {
        private DataContext _context;

        public ProductsController([FromServices] DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProducts()
        {
            if(_context.Products != null)
            {
                return Ok(_context.Products);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(long id)
        {
            Product? p = await _context.Products.FindAsync(id);

            if(p != null)
            {
                return Ok(p);
            }
            return NotFound();
        }

        [HttpPost]
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            if(await _context.Products.ContainsAsync(product))
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return Ok(product);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            Product? p = await _context.Products.FindAsync(id);

            if(p != null)
            {
                _context.Products.Remove(p);
                await _context.SaveChangesAsync();
                return Ok(p);
            }
            return NotFound();
        }


        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchProduct(long id, JsonPatchDocument<Product> patchDoc)
        {
            Product? p = await _context.Products.FindAsync(id);

            if(p != null)
            {
                patchDoc.ApplyTo(p);
                await _context.SaveChangesAsync();
                return Ok(p);
            }
            return NotFound();
        }
    }
}
