using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoriesController : Controller
    {
        private DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IAsyncEnumerable<Category>? GetCategories()
        {
            IEnumerable<Category>? categories = _context.Categories.Include(c => c.Products);

            if(categories != null)
            {
                foreach(var c in categories)
                {
                    foreach(var p in c.Products)
                    {
                        p.Category = null;
                    }
                }
            }
            return (IAsyncEnumerable<Category>?)categories;
        }
    }
}
