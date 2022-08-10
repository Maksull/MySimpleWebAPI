using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class CategoriesController : Controller
    {
        private DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategories()
        {
            IEnumerable<Category>? categories = _context.Categories.Include(c => c.Products);

            if (categories != null)
            {
                foreach (var c in categories)
                {
                    foreach (var p in c.Products)
                    {
                        p.Category = null;
                    }
                }
                return Ok(categories);
            }
            return NotFound();
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategory(long id)
        {
            Category? category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category != null)
            {
                foreach (Product p in category.Products)
                {
                    p.Category = null;
                }
                return Ok(category);
            }
            return NotFound();
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveCategory(CategoryBindingTarget categoryBinding)
        {
            if (ModelState.IsValid)
            {
                Category c = categoryBinding.ToCategory();
                await _context.Categories.AddAsync(c);
                await _context.SaveChangesAsync();
                return Ok(c);
            }
            return BadRequest(ModelState);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            if (await _context.Categories.ContainsAsync(category))
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return Ok(category);
            }

            return NotFound();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            Category? c = await _context.Categories.FindAsync(id);

            if (c != null)
            {
                _context.Categories.Remove(c);
                await _context.SaveChangesAsync();
                return Ok(c);
            }

            return NotFound();
        }


        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchCategory(long id, JsonPatchDocument<Category> patchDoc)
        {
            Category? c = await _context.Categories.FindAsync(id);
            
            if(c != null)
            {
                patchDoc.ApplyTo(c);
                await _context.SaveChangesAsync();
                return Ok(c);
            }
            return NotFound();
        }
    }
}
